using System;
using System.Collections.Generic;

namespace HungNT
{
    /// <summary>Lưu tiến trình level của người chơi.</summary>
    [Serializable]
    public class LevelSave : BaseUserData
    {
        /// <summary>Level hiện tại (bắt đầu từ 1).</summary>
        public int CurrentLevel = 1;

        /// <summary>Level cao nhất đã mở khóa.</summary>
        public int MaxUnlockedLevel = 1;

        /// <summary>Tổng số star đã thu thập.</summary>
        public int TotalStars = 0;

        /// <summary>Star của từng level (key = levelId).</summary>
        public Dictionary<int, int> LevelStars = new();

        public bool IsLevelUnlocked(int levelId) => levelId <= MaxUnlockedLevel;

        public int GetStars(int levelId) =>
            LevelStars.TryGetValue(levelId, out var s) ? s : 0;

        public void SetStars(int levelId, int stars)
        {
            int old = GetStars(levelId);
            if (stars > old)
            {
                TotalStars += stars - old;
                LevelStars[levelId] = stars;
            }
        }
    }
}
