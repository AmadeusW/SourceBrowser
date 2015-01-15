using System;
using System.Diagnostics;

namespace SourceBrowser.Utils
{
    public class LoggingUtils
    {
        private const string CATEGORY_ERROR = "Error";
        private const string CATEGORY_WARNING = "Warning";
        private const string CATEGORY_INFO = "Info";

        /// <summary>
        /// Logs a message in the Error category
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="memberName">(Automatically populated)</param>
        /// <param name="sourceFilePath">(Automatically populated)</param>
        /// <param name="sourceLineNumber">(Automatically populated)</param>
        public static void Error(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string requestId = RequestUtils.GetRequestId().ToString();
            Trace.WriteLine(String.Format("[{0}, {1}] at {2}:{3} ({4}); {5}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message), CATEGORY_ERROR);
        }

        /// <summary>
        /// Logs a message in the Error category and includes the exception information.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="exception">Exception information to attach</param>
        /// <param name="memberName">(Automatically populated)</param>
        /// <param name="sourceFilePath">(Automatically populated)</param>
        /// <param name="sourceLineNumber">(Automatically populated)</param>
        public static void Error(string message,
            Exception exception,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string requestId = RequestUtils.GetRequestId().ToString();
            Trace.WriteLine(String.Format("[{0}, {1}] at {2}:{3} ({4}); {5} \n {6}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message, exception.ToString()), CATEGORY_ERROR);
            System.Web.HttpContext.Current.Trace.Write(String.Format("xxx [{0}, {1}] at {2}:{3} ({4}); {5} \n {6}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message, exception.ToString()), CATEGORY_ERROR);
        }

        /// <summary>
        /// Logs a message in the Warning category
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="memberName">(Automatically populated)</param>
        /// <param name="sourceFilePath">(Automatically populated)</param>
        /// <param name="sourceLineNumber">(Automatically populated)</param>
        public static void Warning(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string requestId = RequestUtils.GetRequestId().ToString();
            Trace.WriteLine(String.Format("[{0}, {1}] at {2}:{3} ({4}); {5}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message), CATEGORY_WARNING);
        }

        /// <summary>
        /// Logs a message in the Info category
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="memberName">(Automatically populated)</param>
        /// <param name="sourceFilePath">(Automatically populated)</param>
        /// <param name="sourceLineNumber">(Automatically populated)</param>
        public static void Info(string message,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            string requestId = RequestUtils.GetRequestId().ToString();
            Trace.WriteLine(String.Format("[{0}, {1}] at {2}:{3} ({4}); {5}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message), CATEGORY_INFO);
            System.Web.HttpContext.Current.Trace.Write(String.Format("xxx [{0}, {1}] at {2}:{3} ({4}); {5}", DateTime.UtcNow, requestId, sourceFilePath, sourceLineNumber, memberName, message), CATEGORY_INFO);
        }
    }
}
