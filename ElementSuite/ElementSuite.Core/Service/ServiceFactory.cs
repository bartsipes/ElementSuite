namespace ElementSuite.Core.Service
{
    using Castle.Windsor;
    using ElementSuite.Common.Interface;
    using ElementSuite.Core.Internal;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ServiceFactory : IServiceLocator
    {
        private static ServiceFactory _serviceFactory;
        private IWindsorContainer _container;

        public static ServiceFactory Instance
        {
            get
            {
                if (_serviceFactory == null)
                    _serviceFactory = new ServiceFactory();

                return _serviceFactory;
            }
        }

        private ServiceFactory()
        {
            _container = Initializer.Instance.WindsorContainer;
        }

        public object Resolve(System.Type serviceType)
        {
            var inherited = serviceType.GetInterfaces().Where(i => i == typeof(IService)).FirstOrDefault();
            if (inherited != null)
                return _container.Resolve(serviceType);
            else
                return null;
        }

        T IServiceLocator.Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}

