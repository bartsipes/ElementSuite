using ElementSuite.Addin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementSuite.Addin.Test
{
    public class TestLaunchMenuExtension : IMenuExtension
    {
        public string Name {
            get { return "Launch Addin"; }
        }

        public System.Windows.Input.ICommand Command { get; private set; }

        public IEnumerable<IMenuExtension> SubMenus { get; private set; }

        public TestLaunchMenuExtension()
        {
            Command = new TestLaunchCommand();
        }
    }
}
