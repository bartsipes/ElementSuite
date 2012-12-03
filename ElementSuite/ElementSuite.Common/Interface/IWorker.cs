namespace ElementSuite.Common.Interface
{
    using System.ServiceModel;

    /// <summary>
    /// Service boundary for the distributed worker.
    /// </summary>
    [ServiceContract()]
    public interface IWorker 
	{
        /// <summary>
        /// Handles a UDP broadcast notification from a client that needs work evaulated.
        /// This is a one way action that returns immediately.
        /// </summary>
        /// <param name="workQueueInfo">Instance of <see cref="ElementSuite.Common.WorkQueueInfo"/> which contains the location of the work queue.</param>
        [OperationContract(IsOneWay = true)]
        void RequestWorkExecution(WorkQueueInfo workQueueInfo);
	}
}