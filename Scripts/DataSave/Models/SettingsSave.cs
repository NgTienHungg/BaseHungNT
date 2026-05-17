// using System;
// using HungNT.Datasave;
// using UnityEngine;
//
// namespace HungNT
// {
//     /// <summary>Lưu cài đặt âm lượng, rung và hiển thị trong một domain file riêng.</summary>
//     [Serializable]
//     public class SettingsSave : BaseSaveData
//     {
//         public override string SaveFileName => "domain_game_settings.es3";
//
//         public override string RootStorageKey => "game_settings";
//
//         public float MasterVolume = 1f;
//
//         public float MusicVolume = 0.7f;
//
//         public float SfxVolume = 1f;
//
//         public bool Vibration = true;
//
//         public bool Notifications = true;
//
//         public string Language = "vi";
//
//         public override void OnAfterLoad()
//         {
//             MasterVolume = Mathf.Clamp01(MasterVolume);
//             MusicVolume = Mathf.Clamp01(MusicVolume);
//             SfxVolume = Mathf.Clamp01(SfxVolume);
//         }
//     }
// }
