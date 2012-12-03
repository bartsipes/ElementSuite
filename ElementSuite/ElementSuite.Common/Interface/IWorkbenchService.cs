namespace ElementSuite.Common.Interface
{
    using System.Collections.Generic;
    using System.Windows.Controls;

    /// <summary>
    /// Exposes the visual workbench of the main application to addins.
    /// </summary>
    public interface IWorkbenchService : IService, IEnumerable<TabItem>
    {
        /// <summary>
        /// Adds the given tab item to the workbench.
        /// </summary>
        /// <param name="tabItem">Tab item to be added to the work bench.</param>
        void Add(TabItem tabItem);

        /// <summary>
        /// Number of tab items already added to the work bench.
        /// </summary>
        int Count { get; }
    }
}
