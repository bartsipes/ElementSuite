using ElementSuite.Addin.Interface;
using ElementSuite.Common;
using ElementSuite.Common.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ElementSuite.Core.Internal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    internal class DistributedAddinWorkQueue<TWorkItem, TWorkResult> : IAddinWorkQueue<TWorkItem, TWorkResult>, IWorkQueue<TWorkItem, TWorkResult>, IWorkQueueContext, IDisposable
            where TWorkItem : IWorkItem
            where TWorkResult : IWorkResult
    {
        private BlockingCollection<TWorkItem> _queuedWork;
        private ConcurrentDictionary<Guid, TWorkItem> _pendingWork;
        private bool _isCompleteRaised;
        private WorkContext workContext;
        private WorkerClient client;
        private int queueContextPort;
        private int queuePort;
        private ServiceHost workQueueContextHost;
        private ServiceHost workQueueHost;
        private readonly ILoggingService _logger;

        private bool _isEnqueuingComplete;
        public bool IsEnqueuingComplete
        {
            get { return _queuedWork.IsAddingCompleted; }
            private set
            {
                _isEnqueuingComplete = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsEnqueuingComplete"));
            }
        }

        private bool _isComplete;
        public bool IsComplete
        {
            get { return _queuedWork.IsCompleted; }
            private set
            {
                _isComplete = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("IsComplete"));
            }
        }

        private ObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>> _backingResults;
        private ReadOnlyObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>> _results;
        public ReadOnlyObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>> Results
        {
            get { return _results; }
        }

        internal DistributedAddinWorkQueue()
        {
            _queuedWork = new BlockingCollection<TWorkItem>();
            _pendingWork = new ConcurrentDictionary<Guid, TWorkItem>();
            _backingResults = new ObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>>();
            _results = new ReadOnlyObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>>(_backingResults);
            _logger = Initializer.Instance.WindsorContainer.Resolve<ILoggingService>();

            PropertyChanged += CompletedHandler;

            if (!Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["ElementSuite.DistributedWorkQueuePort"], out queuePort))
            {
                _logger.Log(LogLevel.Error, "Unable to parse the app setting ElementSuite.DistributedWorkQueuePort. This value should be an integer in the range between 49152 and 65535.");
            }
            if (
                !Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["ElementSuite.DistributedWorkQueueContextPort"], out queueContextPort))
            {
                _logger.Log(LogLevel.Error, ("Unable to parse the app setting ElementSuite.DistributedWorkQueueContextPort. This value should be an integer in the range between 49152 and 65535."));
            }
        }

        public void Enqueue(TWorkItem workItem)
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - Enqueuing called");
            _queuedWork.Add(workItem);
        }

        public void Enqueue(IEnumerable<TWorkItem> workItems)
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - Enqueuing enumerable called");
            foreach (var item in workItems)
            {
                _queuedWork.Add(item);
            }
        }

        public void CompleteEnqueuing()
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - CompleteEnqueuing called");
            _queuedWork.CompleteAdding();

            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("IsEnqueuingComplete"));
                if (_queuedWork.Count == 0)
                {
                    lock (this)
                    {
                        if (!_isCompleteRaised)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("IsComplete"));
                            _isCompleteRaised = true;
                        }
                    }
                }
            }
        }

        public bool Initialize<TWorkCommand>() where TWorkCommand : IWorkCommand
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - Initialize called");
            Type workCommandType = typeof(TWorkCommand);
            Type commandType = null;
            IPAddress localIP = GetMyIP();

            if (localIP == null)
            {
                _logger.Log(LogLevel.Fatal, "Unable to retreive the local ip address");
                throw new NullReferenceException("Unable to retreive the local ip address");
            }
            
            foreach (var command in Initializer.Instance.AddinWorkCommands)
            {
                if (command.Value.GetType() == workCommandType)
                {
                    commandType = command.Value.GetType();
                    break;
                }
            }

            if (commandType != null)
            {
                try
                {
                    workContext = new WorkContext();
                    workContext.WorkCommandFile = File.ReadAllBytes(commandType.Assembly.Location);

                    Uri workQueueContextUri = new Uri(string.Format("net.tcp://{0}:{1}/", localIP.ToString(), queueContextPort));
                    workQueueContextHost = new ServiceHost(this, workQueueContextUri);
                    Binding sharedBinding = new NetTcpBinding();
                    workQueueContextHost.AddServiceEndpoint(typeof(IWorkQueueContext), sharedBinding, workQueueContextUri);
                    workQueueContextHost.Open();
                    _logger.Log(LogLevel.Info, string.Format("AddinWorkQueue - Initialize - Work queue context service opened at \"{0}\".", workQueueContextUri));

                    Uri workQueueUri = new Uri(string.Format("net.tcp://{0}:{1}/", localIP.ToString(), queuePort));
                    workQueueHost = new ServiceHost((IWorkQueue<TWorkItem, TWorkResult>)this, workQueueUri);
                    workQueueHost.AddServiceEndpoint(typeof(IWorkQueue<TWorkItem, TWorkResult>), new NetTcpBinding(), workQueueUri);
                    workQueueHost.Open();
                    _logger.Log(LogLevel.Info, string.Format("AddinWorkQueue - Initialize - Work queue service opened at \"{0}\".", workQueueUri));
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            else
            {
                var errorMessage = string.Format("An assembly containing the type \"{0}\" could not be found. Please ensure all needed assemblies have been placed in the Addins subdirectory", workCommandType.FullName);
                _logger.Log(LogLevel.Error, errorMessage);
                throw new NullReferenceException(errorMessage);
            }
        }

        public void Start()
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - Start called");
            IPAddress localIP = GetMyIP();
            client = new WorkerClient();
            client.Open();
            client.RequestWorkExecution(new WorkQueueInfo()
            {
                WorkQueueContextLocation = new Uri(string.Format("net.tcp://{0}:{1}/", localIP.ToString(), queueContextPort)),
                WorkQueueLocation = new Uri(string.Format("net.tcp://{0}:{1}/", localIP.ToString(), queuePort)),
            });
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - Start end");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private IPAddress GetMyIP()
        {
            IPHostEntry host;
            IPAddress localIP = null;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip;
                }
            }
            return localIP;
        }


        #region IWorkQueue Implementation
        TWorkItem IWorkQueue<TWorkItem, TWorkResult>.GetNext(WorkerInfo executor)
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - GetNext called");
            TWorkItem nextWorkItem = default(TWorkItem);

            if (_queuedWork.TryTake(out nextWorkItem) == false)
            {
                if (_queuedWork.IsAddingCompleted && PropertyChanged != null)
                {
                    lock (this)
                    {
                        if (!_isCompleteRaised)
                        {
                            _logger.Log(LogLevel.Info, "AddinWorkQueue - IsComplete set");
                            PropertyChanged(this, new PropertyChangedEventArgs("IsComplete"));
                            _isCompleteRaised = true;
                        }
                    }
                }
            }
            else
            {
                _pendingWork.AddOrUpdate(nextWorkItem.Id, nextWorkItem, (key, oldValue) => nextWorkItem);
            }

            return nextWorkItem;
        }

        void IWorkQueue<TWorkItem, TWorkResult>.ReturnResult(TWorkResult workResult, WorkerInfo executor)
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - ReturnResult called");
            TWorkItem workItem = default(TWorkItem);
            if (_pendingWork.TryRemove(workResult.WorkItemId, out workItem))
            {
                _backingResults.Add(new ContextualWorkResult(workItem, workResult));
            }
            else
            {
                _logger.Log(LogLevel.Warning, string.Format("Work item id {0} was not found in pending work collection.", workResult.WorkItemId));
            }
        }

        bool IWorkQueue<TWorkItem, TWorkResult>.GetActiveStatus()
        {
            _logger.Log(LogLevel.Debug, string.Format("AddinWorkQueue - GetActiveStatus called; Status is {0}", !_queuedWork.IsCompleted ? "Active" : "Not Active"));
            return !_queuedWork.IsCompleted;
        }
        #endregion

        #region IWorkQueueContext Implementation
        public WorkContext GetContext(WorkerInfo executor)
        {
            _logger.Log(LogLevel.Debug, "AddinWorkQueue - GetContext called");
            return workContext;
        }
        #endregion

        class ContextualWorkResult : IContextualWorkResult<TWorkItem, TWorkResult>
        {
            public ContextualWorkResult(TWorkItem workItem, TWorkResult workResult)
            {
                this.WorkItem = workItem;
                this.WorkResult = workResult;
            }

            public TWorkItem WorkItem { get; private set; }

            public TWorkResult WorkResult { get; private set; }
        }

        /// <summary>
        /// Closes the services when all results have been returned.
        /// </summary>
        void CompletedHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("IsComplete"))
            {
                if (workQueueHost != null)
                    workQueueHost.Close();

                if (workQueueContextHost != null)
                    workQueueContextHost.Close();
            }
        }

        public void Dispose()
        {
            if (workQueueHost != null)
                workQueueHost.Close();

            if (workQueueContextHost != null)
                workQueueContextHost.Close();
        }
    }
}