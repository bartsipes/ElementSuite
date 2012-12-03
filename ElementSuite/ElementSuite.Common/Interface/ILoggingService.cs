namespace ElementSuite.Common.Interface
{
    using System;

    /// <summary>
    /// Service that exposes a variety of methods to log information.
    /// </summary>
    public interface ILoggingService : IService
	{
        /// <summary>
        /// Logs a message with the specifed level and description.
        /// </summary>
        /// <param name="level">Severity level of this particular log.</param>
        /// <param name="description">Description of the log.</param>
		void Log(LogLevel level, string description);

        /// <summary>
        /// Logs a message with the specifed level, description, and exception.
        /// </summary>
        /// <param name="level">Severity level of this particular log.</param>
        /// <param name="description">Description of the log.</param>
        /// <param name="exception">Exception related to the log.</param>
        void Log(LogLevel level, string description, Exception exception);
	}
}