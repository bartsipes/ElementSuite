namespace ElementSuite.Common.Interface
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    /// <summary>
    /// Interface for distributing work. Use the implementing class to store all information necessary
    /// for a distributed worker to complete a unit of work.
    /// </summary>
	public interface IWorkItem 
	{
        /// <summary>
        /// Unique identifier of this work item.
        /// </summary>
        Guid Id { get; }
	}
}

