// using System;
// using System.Collections.Generic;
// using HungNT.Datasave;
//
// namespace HungNT
// {
//     /// <summary>Lưu tiến trình level và sao của từng màn trong một domain file riêng.</summary>
//     [Serializable]
//     public class LevelSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_level_progress.es3";
//
//         public override string RootStorageKey => "levels";
//
//         public int CurrentLevel = 1;
//
//         public int MaxUnlockedLevel = 1;
//
//         public int TotalStars;
//
//         public Dictionary<int, int> LevelStars = new();
//
//         public bool IsLevelUnlocked(int levelId) => levelId <= MaxUnlockedLevel;
//
//         public int GetStars(int levelId) =>
//             LevelStars.TryGetValue(levelId, out var s) ? s : 0;
//
//         public void SetStars(int levelId, int stars)
//         {
//             var old = GetStars(levelId);
//             if (stars > old)
//             {
//                 TotalStars += stars - old;
//                 LevelStars[levelId] = stars;
//             }
//         }
//     }
// }
