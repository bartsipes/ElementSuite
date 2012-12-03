using ElementSuite.Common.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ElementSuite.Addin.Interface
{
    /// <summary>
    /// This interface specifies how an addin may request work to be executed in a distributed fashion.
    /// 
    /// First call the initialize method. This will 
    /// </summary>
    /// <typeparam name="TWorkItem">The specific implementation of the work item that will be executed.</typeparam>
    /// <typeparam name="TWorkResult">The specific implemenation of the work result that will be returned.</typeparam>
    public interface IAddinWorkQueue<TWorkItem, TWorkResult> : INotifyPropertyChanged
        where TWorkItem : IWorkItem
        where TWorkResult : IWorkResult
    {
        /// <summary>
        /// Indicates if the enqueuing period has completed.
        /// </summary>
        bool IsEnqueuingComplete { get; }

        /// <summary>
        /// Indicates if all work items have been completed
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// Collection of work results paired with their respective work items.
        /// </summary>
        ReadOnlyObservableCollection<IContextualWorkResult<TWorkItem, TWorkResult>> Results { get; }

        /// <summary>
        /// Adds a work item to the queue to be evaluated.
        /// </summary>
        /// <param name="workItem">Work item to be added to the queue.</param>
        void Enqueue(TWorkItem workItem);

        /// <summary>
        /// Adds a collection of work items to the queue to be evaluated.
        /// </summary>
        /// <param name="workItems">Work items to be added to the queue.</param>
        void Enqueue(IEnumerable<TWorkItem> workItems);

        /// <summary>
        /// Mark the queue as complete so no other work items may be added.
        /// </summary>
        void CompleteEnqueuing();

        /// <summary>
        /// Initializes the queue with the appropriate work command. If initialization fails, this method may be called again to 
        /// attempt to initialize again.
        /// </summary>
        /// <typeparam name="TWorkCommand">Work command to perform the actual work.</typeparam>
        /// <returns>Bool indicating the success of the initialization.</returns>
        bool Initialize<TWorkCommand>() where TWorkCommand : IWorkCommand;

        /// <summary>
        /// Begin executing the work items that have been enqueued. Initialize must be called before start is called.
        /// </summary>
        void Start();
    }
}
