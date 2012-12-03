namespace ElementSuite.Common
{
    /// <summary>
    /// Severity levels of log records.
    /// </summary>
	public enum LogLevel
	{
        /// <summary>
        /// Most severe log level, the application may close abruptly or not work correctly afterwards.
        /// </summary>
		Fatal = 1,
        /// <summary>
        /// Logs that may indicate a potential issue.
        /// </summary>
		Warning = 3,
        /// <summary>
        /// Includes logs for errors that did not cause a severe problem.
        /// </summary>
		Error = 2,
        /// <summary>
        /// Informative logs, no errors.
        /// </summary>
		Info = 4,
        /// <summary>
        /// Verbose diagnostic logs.
        /// </summary>
		Debug = 5,
	}
}
