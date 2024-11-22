using System;
using System.Collections.Generic;
using System.Linq;
using Shared;
using Debugger;
using Debugger.Services;
using UnityEngine;

namespace Native
{
    /// <summary>
    /// A base class for handling logging with configurable log writers and verbosity levels.
    /// Implements <see cref="Debugger.ILogger"/> for logging functionality.
    /// <para>
    /// This class uses conditional compilation directives to ensure that logging functionality
    /// is only active in editor and debug modes. Specifically, the code within the
    /// <c>#if UNITY_EDITOR || DEBUG</c> directives will not be included in release builds.
    /// </para>
    /// </summary>
    public class BaseLogBehaviour: MonoBehaviour, Debugger.ILogger
    {
        private List<ILogWriter> logWriters;
        private ILogObjectContext<UnityEngine.Object> context;

        /// <summary>
        /// Gets or sets the list of <see cref="ILogWriter"/> instances used to handle log output.
        /// If not set, a default <see cref="UnityConsoleLogWriter"/> is used.
        /// </summary>
        protected List<ILogWriter> LogWriters
        {
            get
            {
                return ValidateAndGetLogWriters();
            }
            private set
            {
                ValidateAndSetLogWriters(value);
            }
        }

        /// <summary>
        /// Gets the log object context for this instance. If the context has not been initialized,
        /// it will be created using the <see cref="UnityObjectContext"/> class.
        /// </summary>
        /// <value>
        /// The log object context associated with this instance.
        /// </value>
        protected ILogObjectContext<UnityEngine.Object> Context
        {
            get
            {
                if (context == null)
                {
                    context = new UnityObjectContext(this, gameObject);
                }

                return context;
            }
        }

        /// <summary>
        /// Gets the verbosity level for logging. Only logs with a verbosity level
        /// equal to or higher than this value will be processed.
        /// </summary>
        [field: SerializeField]
        protected LogVerbosity EnabledLogVerbosity { get; private set; }

        private void Awake()
        {
            LoggingServiceBuilder serviceBuilder = new LoggingServiceBuilder();
            IServiceCollection<ILogWriter> service = serviceBuilder.WithUnityConsoleLogger().WithFileLogger().Build();
            Init(service);
        }

        /// <summary>
        /// Initializes the logging system using the provided logging service collection.
        /// </summary>
        /// <param name="loggingService">The service collection containing log writers. This collection is used to populate the internal list of log writers.</param>
        /// <remarks>
        /// This method retrieves log writers from the <paramref name="loggingService"/> using <see cref="GetlogWriters"/>.
        /// It then clears any existing log entries by calling <see cref="ClearLogEntries"/>.
        /// </remarks>
        protected virtual void Init(IServiceCollection<ILogWriter> loggingService)
        {
            LogWriters = GetlogWriters(loggingService);
            ClearLogEntries();
        }

        /// <summary>
        /// Retrieves a list of log writers from the provided logging service collection.
        /// </summary>
        /// <param name="loggingService">The service collection that provides log writers. Must be of type <see cref="LoggingServices"/>.</param>
        /// <returns>A list of <see cref="ILogWriter"/> instances obtained from the <paramref name="loggingService"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <paramref name="loggingService"/> is null.
        /// </exception>
        /// <exception cref="InvalidCastException">
        /// Thrown if the <paramref name="loggingService"/> cannot be cast to <see cref="LoggingServices"/>.
        /// </exception>
        protected List<ILogWriter> GetlogWriters(IServiceCollection<ILogWriter> loggingService)
        {
            if (loggingService == null)
            {
                ThrowException(new ArgumentNullException(nameof(loggingService), "Logging service collection cannot be null."));
            }

            if(loggingService is LoggingServices services)
            {
                List<ILogWriter> logWriters = loggingService.GetServices().ToList();

                if(!logWriters.Any())
                {
                    throw new ArgumentNullException(nameof(logWriters), "Log writers cannot be null.");
                }

                return logWriters;
            }

            throw new InvalidCastException("The provided logging service collection is not of type LoggingServices.");
        }

        private void ValidateAndSetLogWriters(List<ILogWriter> value)
        {
            if (value == null)
            {
                ThrowException(new ArgumentNullException("Log writers can not be set to null."));
            }

            logWriters = value;
        }

        private List<ILogWriter> ValidateAndGetLogWriters()
        {
            if (logWriters == null)
            {
                ThrowException(new NullReferenceException("There are no log writers provided by the logging service builder."));
            }

            return logWriters;
        }

#region Logging functions.

        /// <summary>
        /// Logs a message using all configured log writers if the specified verbosity level is enabled.
        /// </summary>
        /// <param name="verbosity">The verbosity level of the log entry. Log writers will only log the message if the verbosity level is less than or equal to the enabled verbosity level.</param>
        /// <param name="message">The message format string to be logged.</param>
        /// <param name="args">Optional parameters for the message format string.</param>
        public void Log(LogVerbosity verbosity, string message, params object[] args)
        {
            if (EnabledLogVerbosity < verbosity)
            {
                return;
            }

            foreach (ILogWriter writer in LogWriters)
            {
                writer.Write(Context, verbosity, message, args);
            }
        }

        /// <summary>
        /// Logs an informational message with the specified arguments at the info verbosity level.
        /// </summary>
        /// <param name="message">The message to log. This can include placeholders for formatting.</param>
        /// <param name="args">Arguments to be inserted into the message placeholders.</param>
        /// <remarks>
        /// This method internally uses <see cref="Log"/> with <see cref="LogVerbosity.Info"/> to handle the logging.
        /// </remarks>
        public void LogInfo(string message, params object[] args)
        {
            Log(LogVerbosity.Info, message, args);
        }

        /// <summary>
        /// Logs a warning message with the specified arguments at the warning verbosity level.
        /// </summary>
        /// <param name="message">The message to log. This can include placeholders for formatting.</param>
        /// <param name="args">Arguments to be inserted into the message placeholders.</param>
        /// <remarks>
        /// This method internally uses <see cref="Log"/> with <see cref="LogVerbosity.Warning"/> to handle the logging.
        /// </remarks>
        public void LogWarning(string message, params object[] args)
        {
            Log(LogVerbosity.Warning, message, args);
        }

        /// <summary>
        /// Logs an error message with the specified arguments at the error verbosity level.
        /// </summary>
        /// <param name="message">The message to log. This can include placeholders for formatting.</param>
        /// <param name="args">Arguments to be inserted into the message placeholders.</param>
        /// <remarks>
        /// This method internally uses <see cref="Log"/> with <see cref="LogVerbosity.Error"/> to handle the logging.
        /// </remarks>
        public void LogError(string message, params object[] args)
        {
            Log(LogVerbosity.Error, message, args);
        }

        /// <summary>
        /// Throws the provided exception after logging it using all configured log writers.
        /// </summary>
        /// <param name="exception">The exception to be thrown and logged. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="exception"/> is null.</exception>
        /// <exception cref="Exception">The provided exception is re-thrown after being logged.</exception>
        public void ThrowException(Exception exception)
        {
            if(exception == null)
            {
                throw new ArgumentNullException(nameof(exception), "Exception cannot be null.");
            }

            foreach (ILogWriter writer in LogWriters)
            {
                writer.WriteException(Context, exception);
            }

            throw exception;
        }

        /// <summary>
        /// Clears all log entries from the configured log writers.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown if no log writers are found in the <see cref="logWriters"/> collection.
        /// </exception>
        public void ClearLogEntries()
        {
            if (logWriters == null || !logWriters.Any())
            {
                ThrowException(new InvalidOperationException("There were no log writers found."));
            }

            foreach (ILogWriter logWriter in logWriters)
            {
                logWriter.Clear();
            }
        }

#endregion
    }
}
