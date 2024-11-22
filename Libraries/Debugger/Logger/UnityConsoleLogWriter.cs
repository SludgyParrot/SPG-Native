using System;
using UnityEngine;

namespace Debugger
{
    /// <summary>
    /// A log writer implementation that outputs log messages to the Unity Console.
    /// Implements the <see cref="ILogWriter"/> interface for writing log entries and exceptions.
    /// </summary>
    public class UnityConsoleLogWriter: ILogWriter
    {
        /// <summary>
        /// Writes a log message to the Unity Console.
        /// </summary>
        /// <typeparam name="T">The type of the log object context.</typeparam>
        /// <param name="objectContext">The log object context containing information about the log source.</param>
        /// <param name="verbosity">The verbosity level of the log entry. Determines how the log message will be handled.</param>
        /// <param name="log">The format string for the log message.</param>
        /// <param name="args">Optional parameters to format the log message.</param>
        /// <remarks>
        /// The method formats the log message based on the <paramref name="verbosity"/> level and writes it to the Unity Console.
        /// Uses <see cref="Debug.LogFormat"/>, <see cref="Debug.LogWarningFormat"/>, <see cref="Debug.LogErrorFormat"/>, or <see cref="Debug.LogAssertionFormat"/>
        /// depending on the specified verbosity level.
        /// </remarks>
        public void Write<T>(ILogObjectContext<T> objectContext, LogVerbosity verbosity, string log, params object[] args)
        {
            //string formattedLogString = LogFormat.GetConsoleLogFormatedString(verbosity, log, this);

            switch (verbosity)
            {
                case LogVerbosity.Assertion:
                    Debug.LogAssertionFormat(log, args);
                    break;
                case LogVerbosity.Info:
                    Debug.LogFormat(log, args);
                    break;
                case LogVerbosity.Warning:
                    Debug.LogWarningFormat(log, args);
                    break;
                case LogVerbosity.Error:
                    Debug.LogErrorFormat(log, args);
                    break;
            }
        }

        /// <summary>
        /// Writes an exception to the Unity Console.
        /// </summary>
        /// <typeparam name="T">The type of the log object context.</typeparam>
        /// <param name="objectContext">The log object context containing information about the log source.</param>
        /// <param name="exception">The exception to be logged.</param>
        /// <remarks>
        /// Uses <see cref="Debug.LogException"/> to output the exception details to the Unity Console.
        /// The <paramref name="objectContext"/> is used to specify the Unity object that the exception is associated with, if applicable.
        /// </remarks>
        public void WriteException<T>(ILogObjectContext<T> objectContext, Exception exception)
        {
            Debug.LogException(exception, objectContext.Context as UnityEngine.Object);
        }

        /// <summary>
        /// Clears all log entries. This implementation does not perform any operations.
        /// </summary>
        /// <remarks>
        /// This method is a placeholder and does not perform any actions in the current implementation.
        /// It can be overridden in derived classes if necessary to implement log clearing functionality.
        /// </remarks>
        public void Clear()
        {

        }
    }
}
