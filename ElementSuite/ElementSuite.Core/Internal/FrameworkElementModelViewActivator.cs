using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ComponentActivator;
using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElementSuite.Core.Internal
{
    internal class FrameworkElementModelViewActivator : DefaultComponentActivator
    {
        public FrameworkElementModelViewActivator(ComponentModel model, IKernel kernel, ComponentInstanceDelegate onCreation, ComponentInstanceDelegate onDestruction) :  base(model, kernel, onCreation, onDestruction)
        {

        }

        protected override object CreateInstance(Castle.MicroKernel.Context.CreationContext context, Castle.Core.ConstructorCandidate constructor, object[] arguments)
        {
            var component = base.CreateInstance(context, constructor, arguments);

            AssignViewModel(component, arguments);

            return component;
        }

        private void AssignViewModel(object component, object[] arguments)
        {
            var frameworkElement = component as FrameworkElement;
            if (frameworkElement == null || arguments == null)
            {
                return;
            }

            var vm = arguments.Where(a => a is IViewModel).FirstOrDefault();
            if (vm != null)
            {
                frameworkElement.DataContext = vm;
            }
        }
    }
}
