namespace ElementSuite.Common
{
	using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines any necessary information for the distributed clients to access the work queue.
    /// </summary>
    [DataContract()]
	public sealed class WorkQueueInfo 
	{
        /// <summary>
        /// Url of the work queue context service.
        /// </summary>
        [DataMember]
        public Uri WorkQueueContextLocation { get; set; }
        /// <summary>
        /// Url of the work queue service.
        /// </summary>
        [DataMember]
        public Uri WorkQueueLocation { get; set; }
    }
}

