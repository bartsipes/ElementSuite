namespace ElementSuite.Common.Interface
{
    /// <summary>
    /// Defines the operation that will allow distributed work to be executed remotely. Classes that implement this interface
    /// must also place the <see cref="System.ComponentModel.Composition.ExportAttribute"/> on the class
    /// and provide the type of <see cref="ElementSuite.Common.Interface.IWorkCommand"/> as a parameter.
    /// <code>
    /// [Export(typeof(IWorkCommand))]
    /// public class ImplementingWorkCommand : IWorkCommand {
    ///     ...
    /// }
    /// </code>
    /// </summary>
    public interface IWorkCommand 
	{
        /// <summary>
        /// Evaluates the given work item and provides the corresponding result.
        /// </summary>
        /// <param name="workItem">Work item to be evaluated.</param>
        /// <returns>Result of the work item evaluation.</returns>
		IWorkResult Execute(IWorkItem workItem);
	}
}