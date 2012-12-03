namespace ElementSuite.Common.Interface
{
    using ElementSuite.Common;
    using System.ServiceModel;

    /// <summary>
    /// Exposes the means for a distributed client to retrieve work to process and return the result.
    /// </summary>
    /// <typeparam name="TWorkItem">Type of the work item specific to the work in the instance of the work queue.</typeparam>
    /// <typeparam name="TWorkResult">Type of the work result specific to the work in the instance of the work queue.</typeparam>
    [ServiceContract()]
    public interface IWorkQueue<TWorkItem, TWorkResult>
        where TWorkItem : IWorkItem
        where TWorkResult : IWorkResult
	{
        /// <summary>
        /// Gets the next work item to be processed.
        /// </summary>
        /// <param name="executor">Identity of the worker that will be evaluating the work item.</param>
        /// <returns>Next work item in the queue.</returns>
        [OperationContract()]
        TWorkItem GetNext(WorkerInfo executor);

        /// <summary>
        /// Accepts the results of an evaluted work item.
        /// </summary>
        /// <param name="workResult">Result of a evaluated work item.</param>
        /// <param name="executor">Identity of the worker that evaluated the work item.</param>
        [OperationContract()]
        void ReturnResult(TWorkResult workResult, WorkerInfo executor);

        /// <summary>
        /// Indicates if the queue is still active with work items to be evaluated.
        /// </summary>
        /// <returns>Status of the queue.</returns>
        [OperationContract()]
        bool GetActiveStatus();
	}
}