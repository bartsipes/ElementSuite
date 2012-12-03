namespace ElementSuite.Addin.Interface
{
    using ElementSuite.Common.Interface;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Defines a contract for extending the Element Suite application.
    /// </summary>
    public interface IAddin
    {
        /// <summary>
        /// Name of the creator of this addin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Name of the addin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Type of this particular addin.
        /// </summary>
        AddinType AddinType { get; }

        /// <summary>
        /// Menu extension that exposes the entry point for launching the addin when the menu extension is selected.
        /// </summary>
        IMenuExtension Launch { get; }

        /// <summary>
        /// Optional collection of menu extensions that are placed underneath the launch menu extension.
        /// </summary>
        ReadOnlyObservableCollection<IMenuExtension> MenuExtensions { get; }

        /// <summary>
        /// Starting point for the addin. This method will be called automatically by Element Suite after the implementing addin
        /// has been pulled into the app. 
        /// </summary>
        /// <param name="serviceLocator">Instance of the service locator that exposes functionality provided by Element Suite.</param>
        void Initialize(IServiceLocator serviceLocator);
    }
}