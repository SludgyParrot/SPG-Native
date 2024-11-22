using System;

namespace Debugger
{
    /// <summary>
    /// Defines a contract for writing log entries for a specific type of object.
    /// </summary>
    /// <typeparam name="T">The type of object that is being logged.</typeparam>
    public interface ILogWriter
    {
        /// <summary>
        /// Writes a log entry with the specified verbosity level and format.
        /// </summary>
        /// <param name="objectContext">The context of the object being logged.</param>
        /// <param name="verbosity">The verbosity level of the log entry.</param>
        /// <param name="log">The log message format string.</param>
        /// <param name="args">Optional parameters for the log message format string.</param>
        void Write<T>(ILogObjectContext<T> objectContext, LogVerbosity verbosity, string log, params object[] args);

        /// <summary>
        /// Writes an exception to the log.
        /// </summary>
        /// <param name="objectContext">The context of the object related to the exception.</param>
        /// <param name="exception">The exception to be logged.</param>
        void WriteException<T>(ILogObjectContext<T> objectContext, Exception exception);

        /// <summary>
        /// Clears all logged entries.
        /// </summary>
        void Clear();
    }
}
