namespace ElementSuite.Common
{
    using System;

    /// <summary>
    /// Identifies an instance of a distributed worker.
    /// </summary>
    [Serializable]
    public sealed class WorkerInfo
    {
        /// <summary>
        /// Name of this instances of a distributed worker.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Unique identifier of this instance of a distributed worker.
        /// </summary>
        public string Id { get; set; }
    }
}
