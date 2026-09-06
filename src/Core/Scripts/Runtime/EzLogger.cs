using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Racer.EzUtilities.Core.Scripts.Runtime
{
    /// <summary>
    /// A static utility class for logging messages and exceptions in Unity projects.
    /// Provides functionality to log messages, store logs, and write them to a file.
    /// </summary>
    public static class EzLogger
    {
#pragma warning disable CS0414 // Field is assigned but its value is never used
        /// <summary>
        /// Indicates whether stack traces should be included in exception logs.
        /// </summary>
        public static bool ShowStackTrace { get; set; } = false;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        /// <summary>
        /// The default filename for the log file.
        /// </summary>
        private static string _logFilename = "logs.txt";

        /// <summary>
        /// A list to store log messages temporarily before writing them to a file.
        /// </summary>
        private static List<string> _logMessages = new();

        /// <summary>
        /// The full file path where logs will be stored.
        /// </summary>
        private static string _logFilePath;

        /// <summary>
        /// Static constructor to initialize the log file path based on the platform.
        /// </summary>
        static EzLogger()
        {
#if UNITY_EDITOR
            _logFilePath = Path.Combine(Application.dataPath, _logFilename);
#else
            _logFilePath = Path.Combine(Application.persistentDataPath, _logFilename);
#endif
        }

        /// <summary>
        /// Logs an exception. In the Unity Editor, logs a warning. In builds, stores the log message.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public static void Log(Exception exception)
        {
#if UNITY_EDITOR
            Debug.LogWarning(exception.Message);
#else
            StoreLogs(
                $"[{LogType.Exception}][{DateTime.Now}]\nMessage: {exception.Message}\nStackTrace: {(ShowStackTrace ? exception.StackTrace : "false")}\n");
#endif
        }

        /// <summary>
        /// Logs a message with a specified log type. In the Unity Editor, logs directly. In builds, stores the log message.
        /// </summary>
        /// <param name="logType">The type of log (e.g., Error, Warning, Info).</param>
        /// <param name="message">The message to log.</param>
        public static void Log(LogType logType, object message)
        {
#if UNITY_EDITOR
            Debug.unityLogger.Log(logType, message);
#else
            StoreLogs($"[{logType}][{DateTime.Now}]\nMessage: {message}\n");
#endif
        }

        /// <summary>
        /// Stores a log message in the internal list for later writing to a file.
        /// </summary>
        /// <param name="message">The log message to store.</param>
        public static void StoreLogs(string message)
        {
            _logMessages.Add(message);
        }

        /// <summary>
        /// Writes all stored log messages to the log file asynchronously and clears the internal list.
        /// </summary>
        public static async Task CommitLogsToFile()
        {
            try
            {
                if (_logMessages.Count == 0) return;

                await File.AppendAllLinesAsync(_logFilePath, _logMessages);
                _logMessages.Clear();
            }
            catch (Exception)
            {
                Debug.LogError("Failed to write logs to file.\nContact developer");
            }
        }

        /// <summary>
        /// Gets the full file path of the log file.
        /// </summary>
        /// <returns>The log file path.</returns>
        public static string GetLogFilePath() => _logFilePath;

#if !UNITY_EDITOR
        /// <summary>
        /// Clears the log file and the internal list of log messages. 
        /// This method is executed before the scene loads in builds.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void ClearLogs()
        {
            try
            {
                if (File.Exists(_logFilePath))
                    File.WriteAllText(_logFilePath, string.Empty);

                _logMessages.Clear();
            }
            catch (Exception)
            {
                Debug.LogError("Failed to clear logs\nContact developer");
            }
        }
#endif
    }
}