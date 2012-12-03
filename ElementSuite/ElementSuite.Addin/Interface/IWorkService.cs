namespace ElementSuite.Addin.Interface
{
    using ElementSuite.Common.Interface;

    /// <summary>
    /// Exposes a method for creating a strongly typed instance of a distributed work queue.
    /// </summary>
    public interface IWorkService : IService
    {
        /// <summary>
        /// Creates a strongly typed instance of a distributed work queue using the specific types needed 
        /// by the addin.
        /// </summary>
        /// <typeparam name="TWorkItem">Type specifing the specific work item type the addin is utilizing.</typeparam>
        /// <typeparam name="TWorkResult">Type specifing the specific work item result type the addin is utilizing.</typeparam>
        /// <returns></returns>
        IAddinWorkQueue<TWorkItem, TWorkResult> CreateWorkQueue<TWorkItem, TWorkResult>()
            where TWorkItem : IWorkItem
            where TWorkResult : IWorkResult;
    }
}