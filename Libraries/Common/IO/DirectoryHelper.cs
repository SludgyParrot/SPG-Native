using System;
using System.IO;
using UnityEngine;

namespace Common.IO
{
    /// <summary>
    /// Provides utility methods for handling directories and file paths.
    /// </summary>
    public static class DirectoryHelper
    {
        #region Debugger Directories

        private const string LogFolderName = "Logs";

        /// <summary>
        /// Gets the directory path for the logs folder. If the directory does not exist,
        /// it will be created.
        /// </summary>
        /// <returns>
        /// The full path to the logs directory.
        /// </returns>
        public static string GetLogFolderDirectory()
        {
            string directory = Path.Combine(Environment.CurrentDirectory, LogFolderName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        /// <summary>
        /// Gets the full path for the log file, including the file name based on the product name.
        /// </summary>
        /// <returns>
        /// The full path to the log file, including the file name.
        /// </returns>
        public static string GetLogFileOutputPath()
        {
            string fileName = $"{Application.productName}-Logs.txt";
            return Path.Combine(GetLogFolderDirectory(), fileName); ;
        }

        #endregion
    }
}
