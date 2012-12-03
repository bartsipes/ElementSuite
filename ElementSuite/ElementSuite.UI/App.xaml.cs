using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ElementSuite.Core.Internal;
using ElementSuite.Core.Interface;
using ElementSuite.Common.Interface;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ElementSuite.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private AppWorkbench workBench;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            workBench = new AppWorkbench(new ObservableCollection<TabItem>());
            Initializer.Initialize(workBench);

            var startupView = Initializer.Instance.WindsorContainer.Resolve<IStartupView>();
            startupView.ShowDialog();
        }

        internal AppWorkbench WorkBench
        {
            get { return workBench; }
        }

        internal class AppWorkbench : IWorkbenchService, INotifyCollectionChanged, INotifyPropertyChanged
        {
            private ObservableCollection<TabItem> tabItemcollection;

            public AppWorkbench(ObservableCollection<TabItem> tabItemcollection)
            {
                this.tabItemcollection = tabItemcollection;
            }

            public void Add(TabItem tabItem)
            {
                tabItemcollection.Add(tabItem);
            }

            public void Remove(TabItem tabItem)
            {
                tabItemcollection.Remove(tabItem);
            }

            public int Count
            {
                get { return tabItemcollection.Count; }
            }

            IEnumerator<TabItem> IEnumerable<TabItem>.GetEnumerator()
            {
                return tabItemcollection.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return tabItemcollection.GetEnumerator();
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add { tabItemcollection.CollectionChanged += value; }
                remove { tabItemcollection.CollectionChanged -= value; }
            }

            public event PropertyChangedEventHandler PropertyChanged
            {
                add { ((INotifyPropertyChanged)tabItemcollection).PropertyChanged += value; }
                remove { ((INotifyPropertyChanged)tabItemcollection).PropertyChanged -= value; }
            }
        }
    }
}
