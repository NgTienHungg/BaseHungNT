using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>Lưu cài đặt âm thanh / hiển thị.</summary>
    [Serializable]
    public class SettingsSave : BaseUserData
    {
        public float MasterVolume = 1f;
        public float MusicVolume = 0.7f;
        public float SfxVolume = 1f;
        public bool Vibration = true;
        public bool Notifications = true;
        public string Language = "vi";

        public override void OnAfterLoad()
        {
            MasterVolume = Mathf.Clamp01(MasterVolume);
            MusicVolume  = Mathf.Clamp01(MusicVolume);
            SfxVolume    = Mathf.Clamp01(SfxVolume);
        }
    }
}
