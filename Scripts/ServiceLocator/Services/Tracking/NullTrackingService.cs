using System.Collections.Generic;

namespace HungNT
{
    /// <summary>
    /// Null Object implementation của ITrackingService.
    /// No-op hoàn toàn — dùng trong Editor hoặc môi trường không có analytics SDK.
    /// </summary>
    public class NullTrackingService : ITrackingService
    {
        public void TrackEvent(string eventName)
        {
        }

        public void TrackEvent(string eventName, Dictionary<string, object> parameters)
        {
        }

        public void SetUserId(string userId)
        {
        }

        public void SetUserProperty(string key, string value)
        {
        }
    }
}
