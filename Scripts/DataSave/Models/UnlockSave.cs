// using System;
// using System.Collections.Generic;
// using HungNT.Datasave;
//
// namespace HungNT
// {
//     /// <summary>Lưu tập id đã mở khoá nội dung trong một domain file riêng.</summary>
//     [Serializable]
//     public class UnlockSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_unlocks.es3";
//
//         public override string RootStorageKey => "unlocks";
//
//         public HashSet<string> UnlockedIds = new();
//
//         public bool IsUnlocked(string id) => UnlockedIds.Contains(id);
//
//         public void Unlock(string id) => UnlockedIds.Add(id);
//
//         public void Lock(string id) => UnlockedIds.Remove(id);
//
//         public override void OnAfterLoad()
//         {
//             UnlockedIds ??= new HashSet<string>();
//         }
//     }
// }
