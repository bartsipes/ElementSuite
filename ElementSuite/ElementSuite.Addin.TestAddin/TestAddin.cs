using ElementSuite.Addin.Interface;
using ElementSuite.Common;
using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElementSuite.Addin.Test
{
    [System.ComponentModel.Composition.Export(typeof(ElementSuite.Addin.Interface.IAddin))]
    public class TestAddin : IAddin
    {
        private ObservableCollection<IMenuExtension> backingMenuExtension;
        private ReadOnlyObservableCollection<IMenuExtension> menuExtensions;
        private IMenuExtension launch;
        private IServiceLocator serviceFactory;

        public TestAddin()
        {
            backingMenuExtension = new ObservableCollection<IMenuExtension>();
            menuExtensions = new ReadOnlyObservableCollection<IMenuExtension>(backingMenuExtension);
        }

        public string Author
        {
            get { return "Bart Sipes"; }
        }

        public string Name
        {
            get { return this.GetType().FullName; }
        }

        public AddinType AddinType
        {
            get { return Addin.AddinType.Earth; }
        }

        public IMenuExtension Launch
        {
            get { return launch; }
        }

        public ReadOnlyObservableCollection<IMenuExtension> MenuExtensions
        {
            get { return menuExtensions; }
        }

        public void Initialize(IServiceLocator serviceFactory)
        {
            this.serviceFactory = serviceFactory;

            launch = new MenuExtension() {
                Command = new RelayCommand((obj) => {
                              var workBench = serviceFactory.Resolve<IWorkbenchService>();
                              var tabItem = new WorkbenchTab("Test Addin Title");
                              tabItem.Content = new TestUserControl(serviceFactory);
                              workBench.Add(tabItem);
                          }),
                Name = "Test Addin"
            };
        }

        private class MenuExtension : IMenuExtension
        {

            public string Name { get; set; }

            public ICommand Command { get; set; }

            public IEnumerable<IMenuExtension> SubMenus { get; set; }
        }
    }
}
