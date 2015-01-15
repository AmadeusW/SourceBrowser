using System;
using System.Threading;

namespace SourceBrowser.Utils
{
    public class RequestUtils
    {
        private const string REQUEST_ID_KEY = "RequestId";

        [ThreadStatic]
        private static Guid _requestId;

        /// <summary>
        /// Creates a new RequestId and stores it in current HttpContext/Thread
        /// </summary>
        public static void CreateRequestId()
        {
            Guid requestId = Guid.NewGuid();
            // TODO: Pick one:
            System.Web.HttpContext.Current.Items.Add(REQUEST_ID_KEY, requestId);
            Thread.SetData(Thread.GetNamedDataSlot(REQUEST_ID_KEY), requestId);
            _requestId = requestId;
        }

        /// <summary>
        /// Retrieves a RequestId from current HttpContext/Thread
        /// </summary>
        /// <returns>Guid associated with current HttpContext/Thread, or Guid.Empty if no Guid was found.</returns>
        public static Guid GetRequestId()
        {
            // TODO: Pick one:
            Guid result1;
            Guid result2;
            Guid result3;
            try
            {
                result3 = _requestId;
                result1 = (Guid)System.Web.HttpContext.Current.Items[REQUEST_ID_KEY];
                result2 = (Guid)Thread.GetData(Thread.GetNamedDataSlot(REQUEST_ID_KEY));
            }
            catch (InvalidCastException)
            {
                return Guid.Empty;
            }

            if (result3.Equals(result2) && result3.Equals(result1))
            {
                return result3;
            }
            else
            {
                throw new InvalidOperationException("The three RequestIds don't match: " + result1.ToString() + " and " + result2.ToString() + " and " + result3.ToString());
            }
        }
    }
}
