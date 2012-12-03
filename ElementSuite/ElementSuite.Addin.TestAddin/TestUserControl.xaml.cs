using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElementSuite.Addin.Test
{
    /// <summary>
    /// Interaction logic for TestUserControl.xaml
    /// </summary>
    public partial class TestUserControl : UserControl
    {
        private IServiceLocator serviceFactory;

        public TestUserControl(IServiceLocator serviceFactory)
        {
            InitializeComponent();

            this.serviceFactory = serviceFactory;
            DataContext = new TestViewModel(serviceFactory);
        }
    }
}
