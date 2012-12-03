namespace ElementSuite.Common.Interface
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// Describes the distributed work result. Use the implementing class to store all information necessary
    /// to retrieve the result of a completed unit of work.
    /// </summary>
	public interface IWorkResult 
	{
        /// <summary>
        /// Indicates if the work was able to be successfully calculated.
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// The error that occurred if <see cref="Success">Success</see> is false.
        /// </summary>
        Exception Error { get; }

        /// <summary>
        /// Unique identifier of the corresponding work item.
        /// </summary>
        Guid WorkItemId { get; set; }
	}
}

