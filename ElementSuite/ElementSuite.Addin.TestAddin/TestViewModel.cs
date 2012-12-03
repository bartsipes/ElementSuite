using ElementSuite.Addin.Interface;
using ElementSuite.Addin.TestDistributed;
using ElementSuite.Common;
using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ElementSuite.Addin.Test
{
    public class TestViewModel
    {
        private IServiceLocator serviceFactory;
        private IWorkService workService;
        private ILoggingService loggingService;
        private IResourceStore resources;
        private IAddinWorkQueue<MatrixMultiplicationWorkItem, MatrixMultiplicationWorkResult> workQueue;
        private List<DistributedHelper> helpers;

        public TestViewModel(IServiceLocator serviceFactory)
        {
            this.serviceFactory = serviceFactory;
            loggingService = serviceFactory.Resolve<ILoggingService>();
            var resourceService = serviceFactory.Resolve<IResourceService>();
            resources = resourceService.RetrieveResourceStore(this.GetType());
            resources.SetInt("Test", 5);

            GridA = new List<MatrixRow>();
            GridB = new List<MatrixRow>();
            GridC = new ObservableCollection<MatrixRow>();

            var r = new Random();
            for (int i = 0; i < 3; i++)
            {
                GridA.Add(new MatrixRow(r.Next(100), r.Next(100), r.Next(100)));
                GridB.Add(new MatrixRow(r.Next(100), r.Next(100), r.Next(100)));
                GridC.Add(new MatrixRow(null, null, null));
            }

            CalculateCommand = new RelayCommand(HandleCalculateCommand);
        }

        public List<MatrixRow> GridA { get; set; }
        public List<MatrixRow> GridB { get; set; }
        public ObservableCollection<MatrixRow> GridC { get; set; }
        public ICommand CalculateCommand { get; set; }

        private void HandleCalculateCommand(object obj)
        {
            workService = serviceFactory.Resolve<IWorkService>();
            workQueue = workService.CreateWorkQueue<MatrixMultiplicationWorkItem, MatrixMultiplicationWorkResult>();
            workQueue.Initialize<MatrixMultiplicationCommand>();

            CalculateDistributed();
        }

        public class MatrixRow : INotifyPropertyChanged
        {
            public MatrixRow(int? column1, int? column2, int? column3)
            {
                this.Column1 = column1;
                this.Column2 = column2;
                this.Column3 = column3;
            }

            int? _column1;
            public int? Column1
            {
                get
                {
                    return _column1;
                }
                set {
                    _column1 = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Column1"));
                }
            }
            int? _column2;
            public int? Column2
            {
                get
                {
                    return _column2;
                }
                set
                {
                    _column2 = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Column2"));
                }
            }
            int? _column3;
            public int? Column3
            {
                get
                {
                    return _column3;
                }
                set
                {
                    _column3 = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Column3"));
                }
            }

            public int? this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0:
                            return Column1;
                        case 1:
                            return Column2;
                        case 2:
                            return Column3;
                        default:
                            return null;
                    }
                }
                set
                {
                    switch (i)
                    {
                        case 0:
                            Column1 = value;
                            break;
                        case 1:
                            Column2 = value;
                            break;
                        case 2:
                            Column3 = value;
                            break;
                        default:
                            break;
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }

        class DistributedHelper
        {
            public DistributedHelper()
            {
                WorkItems = new List<MatrixMultiplicationWorkItem>();
            }
            public int FirstIndex { get; set; }
            public int SecondIndex { get; set; }
            public List<MatrixMultiplicationWorkItem> WorkItems { get; set; }
        }

        private void CalculateDistributed()
        {
            int s = GridA.Count;
            helpers = new List<DistributedHelper>();

            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s; j++)
                {
                    DistributedHelper helper = new DistributedHelper();

                    helper.FirstIndex = i;
                    helper.SecondIndex = j;

                    for (int k = 0; k < s; k++)
                    {
                        helper.WorkItems.Add(new MatrixMultiplicationWorkItem() { GridAValue = GridA[i][k].Value, GridBValue = GridB[k][j].Value });
                    }

                    helpers.Add(helper);
                }
            }

            var t1 = new List<MatrixMultiplicationWorkItem>();
            foreach (var t in helpers)
            {
                t1.AddRange(t.WorkItems);
            }

            var t2 = helpers.SelectMany(dh => dh.WorkItems);
            
            workQueue.Enqueue(helpers.SelectMany(dh => dh.WorkItems));
            workQueue.CompleteEnqueuing();
            ((INotifyCollectionChanged)workQueue.Results).CollectionChanged += HandleDistributedResult;
            workQueue.Start();

        }

        void HandleDistributedResult(object sender, NotifyCollectionChangedEventArgs e)
        {
            loggingService.Log(LogLevel.Debug, "TestViewModel.HandleDistributedResult begin");
            foreach (var item in e.NewItems)
            {
                var result = item as IContextualWorkResult<MatrixMultiplicationWorkItem, MatrixMultiplicationWorkResult>;
                var helper = helpers.First(h => h.WorkItems.Where(i => i.Id == result.WorkItem.Id).Any());
                loggingService.Log(LogLevel.Debug, string.Format("Work item id: {0}; Result work item id: {1};", result.WorkItem.Id, result.WorkResult.WorkItemId));
                loggingService.Log(LogLevel.Debug, string.Format("Helper first index: {0}; Helper second index: {1};", helper.FirstIndex, helper.SecondIndex));
                loggingService.Log(LogLevel.Debug, string.Format("Before set: GridC[{0}][{1}] = {2}", helper.FirstIndex, helper.SecondIndex, GridC[helper.FirstIndex][helper.SecondIndex].HasValue ? GridC[helper.FirstIndex][helper.SecondIndex].Value : -1));
                loggingService.Log(LogLevel.Debug, string.Format("{0} x {1} = {2}", result.WorkItem.GridAValue, result.WorkItem.GridBValue, result.WorkResult.Result));
                if (!GridC[helper.FirstIndex][helper.SecondIndex].HasValue)
                    GridC[helper.FirstIndex][helper.SecondIndex] = 0;
                GridC[helper.FirstIndex][helper.SecondIndex] += result.WorkResult.Result;
                loggingService.Log(LogLevel.Debug, string.Format("Before set: GridC[{0}][{1}] = {2}", helper.FirstIndex, helper.SecondIndex, GridC[helper.FirstIndex][helper.SecondIndex].HasValue ? GridC[helper.FirstIndex][helper.SecondIndex].Value : -1));
            }
            loggingService.Log(LogLevel.Debug, "TestViewModel.HandleDistributedResult end");
        }
    }
}
