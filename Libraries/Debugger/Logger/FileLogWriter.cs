using System;
using System.Text;
using System.IO;

using static Shared.IO.DirectoryHelper;

namespace Debugger
{
    /// <summary>
    /// A log writer implementation that outputs log messages to a file.
    /// Implements the <see cref="ILogWriter"/> interface for writing log entries and clearing logs.
    /// </summary>
    public class FileLogWriter : ILogWriter
    {
        private string _outputPath;

        /// <summary>
        /// Gets or sets the path to the log file.
        /// If not set, a default path is generated based on the application name and a "Logs" directory.
        /// </summary>
        public string OutputPath
        {
            get
            {
                if (_outputPath == null)
                {
                    _outputPath = GetLogFileOutputPath();
                }

                return _outputPath;
            }
            set 
            {
                // TODO - Validate directory  first.
                _outputPath = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileLogWriter"/> class with an optional output path.
        /// If no path is provided, a default path will be used.
        /// </summary>
        /// <param name="outputPath">The optional path to the log file. If null or empty, the default path will be used.</param>
        public FileLogWriter(string outputPath = null)
        {
            if(!string.IsNullOrEmpty(outputPath))
            {
                OutputPath = outputPath;
            }
        }

        /// <summary>
        /// Writes a log message to the file specified by <see cref="OutputPath"/>.
        /// Creates the file and writes a header if it does not already exist.
        /// </summary>
        /// <typeparam name="T">The type of the log object context.</typeparam>
        /// <param name="objectContext">The log object context containing information about the log source.</param>
        /// <param name="verbosity">The verbosity level of the log entry. Determines how the log message will be handled.</param>
        /// <param name="log">The format string for the log message.</param>
        /// <param name="args">Optional parameters to format the log message.</param>
        /// <remarks>
        /// The method appends the log message to the specified file. The file is created if it does not exist,
        /// and a header is written at the beginning of the file.
        /// </remarks>
        public void Write<T>(ILogObjectContext<T> objectContext, LogVerbosity verbosity, string log, params object[] args)
        {
            if (!File.Exists(OutputPath))
            {
                using (FileStream stream = new FileStream(OutputPath, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string logHeaderTemplate = LogFormat.GetFileLogFormatHeaderTemplate(template: LogFileFormatTemplate.Default, dateTimeFormat: "D");

                        writer.WriteLine(logHeaderTemplate);
                        writer.Close();
                    }

                    stream.Close();
                };
            }

            // Write log message to a file.
            using (var streamWriter = new StreamWriter(path: OutputPath, append: true, encoding: Encoding.UTF8))
            {
                string logContentTemplate = LogFormat.GetFileLogFormatTemplate(objectContext.Context.ToString(), verbosity: verbosity, template: LogFileFormatTemplate.Standard, dateTimeFormat: "hh:mm:ss", logMessage: log, args);

                streamWriter.WriteLine(logContentTemplate);
                streamWriter.Close();
            }
        }

        /// <summary>
        /// Writes an exception to the file specified by <see cref="OutputPath"/>.
        /// This implementation is not yet defined.
        /// </summary>
        /// <typeparam name="T">The type of the log object context.</typeparam>
        /// <param name="objectContext">The log object context containing information about the log source.</param>
        /// <param name="exception">The exception to be logged.</param>
        /// <remarks>
        /// This method is currently a placeholder and does not perform any operations.
        /// </remarks>
        public void WriteException<T>(ILogObjectContext<T> objectContext, Exception exception)
        {
            
        }

        /// <summary>
        /// Deletes the log file specified by <see cref="OutputPath"/>.
        /// </summary>
        /// <remarks>
        /// This method removes the log file if it exists, effectively clearing all logged content.
        /// </remarks>
        public void Clear()
        {
            if (File.Exists(OutputPath))
            {
                File.Delete(OutputPath);
            }
        }
    }
}
