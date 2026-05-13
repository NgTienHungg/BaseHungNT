using System;

namespace HungNT
{
    /// <summary>Lưu thông tin cơ bản của người chơi: tên, avatar, trải nghiệm lần đầu.</summary>
    [Serializable]
    public class UserProfileSave : BaseUserData
    {
        public string Username = "Player";
        public int AvatarId = 0;

        /// <summary>Đã xem tutorial chưa.</summary>
        public bool HasSeenTutorial = false;

        /// <summary>Ngày đầu tiên chơi (ISO 8601).</summary>
        public string FirstPlayDate = string.Empty;

        public override void OnAfterLoad()
        {
            if (string.IsNullOrEmpty(Username)) Username = "Player";
        }
    }
}
