namespace ElementSuite.Addin.Interface
{
    using ElementSuite.Common.Interface;

    /// <summary>
    /// Interface for relating a work item with it's corresponding result after the work item has been evaluated.
    /// </summary>
    /// <typeparam name="TWorkItem">Type specifing the specific work item type the addin is utilizing.</typeparam>
    /// <typeparam name="TWorkResult">Type specifing the specific work item result type the addin is utilizing.</typeparam>
    public interface IContextualWorkResult<TWorkItem, TWorkResult>
        where TWorkItem : IWorkItem
        where TWorkResult : IWorkResult
    {
        /// <summary>
        /// Work item that was evaluated.
        /// </summary>
        TWorkItem WorkItem { get; }

        /// <summary>
        /// Result of the work item evaulation.
        /// </summary>
        TWorkResult WorkResult { get; }
    }
}