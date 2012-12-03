using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ElementSuite.Addin.Interface;
using ElementSuite.Common.Interface;
using ElementSuite.Core.Interface;
using ElementSuite.Core.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ElementSuite.Core.Internal
{
    public sealed class Initializer
    {
        private static Initializer _initializer;
        private CompositionContainer _container;
        private IWindsorContainer _windsorContainer;

        public static Initializer Instance
        {
            get
            {
                return _initializer;
            }
        }

        public IWindsorContainer WindsorContainer
        {
            get
            {
                return _windsorContainer;
            }
        }

        [ImportMany(typeof(IAddin))]
        public IEnumerable<Lazy<IAddin>> Addins { get; set; }

        [ImportMany(typeof(IWorkCommand))]
        public IEnumerable<Lazy<IWorkCommand>> AddinWorkCommands { get; set; }

        private Initializer()
        {
        }

        public static void Initialize(IWorkbenchService workBench)
        {
            _initializer = new Initializer();
            _initializer._windsorContainer = _initializer.BootstrapContainer(workBench);
            var logger = _initializer._windsorContainer.Resolve<ILoggingService>();

            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();

            var currentPath = AppDomain.CurrentDomain.BaseDirectory;
            var addinPath = currentPath + "Addins";

            List<Assembly> addinAssemblies = new List<Assembly>();

            foreach (var path in Directory.EnumerateFiles(addinPath, "*.dll"))
            {
                addinAssemblies.Add(Assembly.LoadFrom(path));
            }

            //Adds all the parts found in the same assembly as the Program class
            foreach (var assembly in addinAssemblies)
            {
                catalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }

            //Create the CompositionContainer with the parts in the catalog
            _initializer._container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _initializer._container.ComposeParts(_initializer);
            }
            catch (CompositionException compositionException)
            {
                logger.Log(Common.LogLevel.Error, "Exception thrown composing addin assemblies.", compositionException);
            }

            //Initialize all of the addins
            if (_initializer.Addins != null)
            {
                foreach (var addin in _initializer.Addins)
                {
                    try
                    {
                        addin.Value.Initialize(ServiceFactory.Instance);
                    }
                    catch (Exception ex)
                    {
                        logger.Log(Common.LogLevel.Error, string.Format("Exception thrown initializing an addin of type \"{0}\".", addin.GetType().FullName), ex);
                    }
                }
            }

        }

        private IWindsorContainer BootstrapContainer(IWorkbenchService workBench)
        {
            var container = new WindsorContainer()
               .Install(Configuration.FromAppConfig(),
                        FromAssembly.InThisApplication()
                //perhaps pass other installers here if needed               
               );

            // Load all of the view models and views
            var assemblyTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());

            var viewModelTypes = from m in assemblyTypes
                                 where typeof(IViewModel).IsAssignableFrom(m)
                                 select m;

            foreach (Type t in viewModelTypes)
            {
                container.Register(Castle.MicroKernel.Registration.Component.For(t).Named(t.FullName).LifestyleTransient());
            }

            var viewTypes = from v in assemblyTypes
                            where typeof(IView).IsAssignableFrom(v) && !typeof(IStartupView).IsAssignableFrom(v)
                            select v;

            foreach (Type t in viewTypes)
            {
                container.Register(Castle.MicroKernel.Registration.Component.For(t).Named(t.FullName).LifestyleTransient().Activator<FrameworkElementModelViewActivator>());
            }

            if (workBench != null)
            {
                container.Register(Castle.MicroKernel.Registration.Component.For(typeof(IWorkbenchService)).Named("WorkBenchService").LifestyleSingleton().Instance(workBench));
            }

            return container;
        }

        /// <summary>
        /// Called when the application is about to be shut down. This will inintialize a teardown sequence for items such as the resource service
        /// which need to save state.
        /// </summary>
        public void Cleanup()
        {
            // Teardown resources
            WindsorContainer.Resolve<IResourceService>().Dispose();
        }
    }
}
