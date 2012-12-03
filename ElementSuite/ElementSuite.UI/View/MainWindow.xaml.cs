using ElementSuite.Addin.Interface;
using ElementSuite.Common;
using ElementSuite.Common.Interface;
using ElementSuite.Core.Interface;
using ElementSuite.Core.Internal;
using ElementSuite.UI.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElementSuite.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IStartupView
    {
        private readonly IWorkbenchService workBench;
        private readonly App.AppWorkbench internalWorkBench;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this.Resources["mainViewModel"] as MainViewModel;

            internalWorkBench = ((App)App.Current).WorkBench;
            internalWorkBench.CollectionChanged += WorkBench_CollectionChanged;

            var intro = new WorkbenchTab("Introduction");
            intro.Content = new IntroductionTab();
            internalWorkBench.Add(intro);

            EventManager.RegisterClassHandler(typeof(MainWindow), WorkbenchTab.CloseEvent, new RoutedEventHandler(CloseTabHandler), true);
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;

            // Handle the closing a different way
            var viewModel = DataContext as MainViewModel;
            viewModel.ExitCommand.Execute(e);
        }

        void WorkBench_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    //MainTabControl.Items.Add(item);
                    var i = item as Control;
                    if (i != null)
                    {
                        viewModel.Tabs.Add(i);
                        i.Focus();
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    var i = item as Control;
                    if (i != null)
                        viewModel.Tabs.Remove(i);
                }
            }
        }


        void CloseTabHandler(object sender, RoutedEventArgs e)
        {
            var tab = e.OriginalSource as WorkbenchTab;
            if (tab != null)
                internalWorkBench.Remove(tab);
        }
    }
}
