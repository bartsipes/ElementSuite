namespace ElementSuite.Common.Interface
{
    using System;

    /// <summary>
    /// Provides a means of accessing instances of the available services.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Resolves a service based on the specified type.
        /// </summary>
        /// <param name="serviceType">The type of the service to retrieve.</param>
        /// <returns>The instance of the service.</returns>
        object Resolve(Type serviceType);

        /// <summary>
        /// Resolves a service based on the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the service to retrieve.</typeparam>
        /// <returns>The strongly typed instance of the service.</returns>
        T Resolve<T>() where T : IService;
    }
}
