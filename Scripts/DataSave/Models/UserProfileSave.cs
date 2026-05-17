// using System;
// using HungNT.Datasave;
//
// namespace HungNT
// {
//     /// <summary>Lưu hồ sơ người chơi tối giản và cờ onboarding trong một domain file riêng.</summary>
//     [Serializable]
//     public class UserProfileSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_user_profile.es3";
//
//         public override string RootStorageKey => "profile";
//
//         public string Username = "Player";
//
//         public int AvatarId;
//
//         public bool HasSeenTutorial;
//
//         public string FirstPlayDate = string.Empty;
//
//         public override void OnAfterLoad()
//         {
//             if (string.IsNullOrEmpty(Username))
//                 Username = "Player";
//         }
//     }
// }
