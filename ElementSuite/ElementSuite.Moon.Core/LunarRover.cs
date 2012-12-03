using ElementSuite.Common;
using ElementSuite.Common.Interface;
using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Timers;

namespace ElementSuite.Moon.Core
{

    public class LunarRover : IWorker
    {
        private Uri _workQueueContextLocation;
        private Uri _workQueueLocation;
        private CompositionContainer _container;
        private Timer _waitTimer;
        private WorkerInfo _workerInfo;

        [Import(typeof(IWorkCommand))]
        protected IWorkCommand WorkCommand { get; set; }

        public LunarRover()
        {
            _workerInfo = new WorkerInfo()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Environment.MachineName
            };
        }

        public void RequestWorkExecution(WorkQueueInfo workQueueInfo)
        {
            _workQueueContextLocation = workQueueInfo.WorkQueueContextLocation;
            _workQueueLocation = workQueueInfo.WorkQueueLocation;

            var workQueueContextAddress = new EndpointAddress(_workQueueContextLocation);
            var workQueueContextChannelFactory = new ChannelFactory<IWorkQueueContext>(new NetTcpBinding(), workQueueContextAddress);
            var workQueueContext = workQueueContextChannelFactory.CreateChannel(workQueueContextAddress);
            var context = workQueueContext.GetContext(_workerInfo);
            ((IChannel)workQueueContext).Close();

            var assembly = Assembly.Load(context.WorkCommandFile);
            Type workItemType = null;
            Type workResultType = null;

            _container = new CompositionContainer(new AssemblyCatalog(assembly));
            _container.ComposeParts(this);

            foreach (var item in assembly.GetTypes())
            {
                if (typeof(IWorkItem).IsAssignableFrom(item))
                {
                    workItemType = item;
                }
                if (typeof(IWorkResult).IsAssignableFrom(item))
                {
                    workResultType = item;
                }
            }

            var workQueueAddress = new EndpointAddress(_workQueueLocation);
            var workQueueChannelFactoryType = typeof(ChannelFactory<>).MakeGenericType(typeof(IWorkQueue<,>).MakeGenericType(workItemType, workResultType));
            var workQueueChannelFactory = Activator.CreateInstance(workQueueChannelFactoryType, new NetTcpBinding(), workQueueAddress);

            var createChannelMethod = workQueueChannelFactory.GetType().GetMethod("CreateChannel", new Type[] { });
            var workQueue = createChannelMethod.Invoke(workQueueChannelFactory, null);

            var workQueueType = workQueue.GetType();
            var getActiveStatus = workQueueType.GetMethod("GetActiveStatus");
            var getNext = workQueueType.GetMethod("GetNext");
            IWorkItem workItem = null;

            while ((bool)getActiveStatus.Invoke(workQueue, null) && (workItem = (IWorkItem)getNext.Invoke(workQueue, new[] { _workerInfo })) != null)
            {
                var returnResult = workQueueType.GetMethod("ReturnResult");
                var workResult = WorkCommand.Execute(workItem);
                // Synchronize the result with the work item.
                workResult.WorkItemId = workItem.Id;
                returnResult.Invoke(workQueue, new object[] { workResult, _workerInfo });
            }

            var close = workQueueChannelFactoryType.GetMethods().Where(_ => _.Name == "Close").First();
            close.Invoke(workQueueChannelFactory, null);
        }
    }
}