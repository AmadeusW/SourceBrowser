using System;
using System.Threading;

namespace SourceBrowser.Utils
{
    public class RequestUtils
    {
        [ThreadStatic]
        private static Guid _requestId;

        /// <summary>
        /// Creates a new RequestId and stores it in current HttpContext/Thread
        /// </summary>
        public static void CreateRequestId()
        {
            _requestId = Guid.NewGuid();
        }

        /// <summary>
        /// Retrieves a RequestId from current HttpContext/Thread
        /// </summary>
        /// <returns>Guid associated with current HttpContext/Thread, or Guid.Empty if no Guid was found.</returns>
        public static Guid GetRequestId()
        {
            return _requestId;
        }
    }
}
