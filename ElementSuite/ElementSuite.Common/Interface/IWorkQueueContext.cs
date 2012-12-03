namespace ElementSuite.Common.Interface
{
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Provides contextual information about the work queue. 
    /// </summary>
    [ServiceContract()]
    public interface IWorkQueueContext
    {
        /// <summary>
        /// Gets the context of the work queue. This is used to retrieve the <see cref="System.Reflection.Assembly"/>
        /// which contains an implementation of <see cref="ElementSuite.Common.Interface.IWorkCommand"/> which can be
        /// used to evaluate work items for the work queue.
        /// </summary>
        /// <param name="executor">Identity of the distributed worker.</param>
        /// <returns>The work context for the work queue.</returns>
        [OperationContract()]
        WorkContext GetContext(WorkerInfo executor);
    }
}