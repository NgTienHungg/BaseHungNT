using System;
using System.Collections.Generic;

namespace HungNT
{
    /// <summary>
    /// Lưu trạng thái unlock của các item / nội dung trong game.
    /// Key = item id (string), value = true nếu đã unlock.
    /// </summary>
    [Serializable]
    public class UnlockSave : BaseUserData
    {
        public HashSet<string> UnlockedIds = new();

        public bool IsUnlocked(string id) => UnlockedIds.Contains(id);

        public void Unlock(string id) => UnlockedIds.Add(id);

        public void Lock(string id) => UnlockedIds.Remove(id);
    }
}
