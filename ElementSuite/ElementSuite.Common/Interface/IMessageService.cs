namespace ElementSuite.Common.Interface
{
    /// <summary>
    /// Service that exposes a variety of methods to visually inform the end user.
    /// </summary>
    public interface IMessageService : IService
	{
        /// <summary>
        /// Display's an error message to the end user. 
        /// </summary>
        /// <param name="message">Error message to be displayed.</param>
		void ShowError(string message);

        /// <summary>
        /// Display's an error message along with an exception to the end user.
        /// </summary>
        /// <param name="message">Error message to be displayed.</param>
        /// <param name="exception">Exception referenced by error message.</param>
		void ShowException(string message, System.Exception exception);

        /// <summary>
        /// Display's a warning message to the end user.
        /// </summary>
        /// <param name="message">Warning message to be displayed.</param>
		void ShowWarning(string message);

        /// <summary>
        /// A blocking call that prompts the user for a boolean response.
        /// </summary>
        /// <param name="message">Question to be diplayed.</param>
        /// <param name="caption">Message to give context to the question.</param>
        /// <returns>The end user's response.</returns>
		bool AskQuestion(string message, string caption);

	}
}

