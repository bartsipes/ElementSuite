using ElementSuite.Addin.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElementSuite.UI
{
    internal static class Extensions
    {
        internal static MenuItem ToMenuItem(this IMenuExtension menuExtension)
        {
            MenuItem result = null;
            if (menuExtension != null)
            {
                result = new MenuItem();
                result.Command = menuExtension.Command;
                result.Header = menuExtension.Name;
            }

            return result;
        }
    }
}
