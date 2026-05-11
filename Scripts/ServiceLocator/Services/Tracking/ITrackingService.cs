using System.Collections.Generic;

namespace HungNT
{
    /// <summary>
    /// Interface cho Analytics/Tracking service.
    /// Implement bằng SDK cụ thể (Firebase Analytics, Adjust, AppsFlyer, ...).
    /// </summary>
    public interface ITrackingService : IService
    {
        /// <summary>Ghi lại một sự kiện không có tham số.</summary>
        void TrackEvent(string eventName);

        /// <summary>Ghi lại một sự kiện kèm các tham số bổ sung.</summary>
        void TrackEvent(string eventName, Dictionary<string, object> parameters);

        /// <summary>Gắn User ID để liên kết các sự kiện với một người dùng cụ thể.</summary>
        void SetUserId(string userId);

        /// <summary>Gắn thuộc tính người dùng (user property / super property).</summary>
        void SetUserProperty(string key, string value);
    }
}
