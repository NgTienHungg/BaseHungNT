using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng DataSaveManager.
    /// <para>Gắn lên GameObject, Play Mode → nhấn các nút trong Inspector.</para>
    /// </summary>
    public class DataSaveDemo : MonoBehaviour
    {
        [Button("Load PlayerSave")]
        public void LoadPlayer()
        {
            var save = DataSaveManager.Instance.Get<DemoPlayerSave>();
            Debug.Log($"[DataSave Demo] Name={save.PlayerName}, Gold={save.Gold}, Level={save.Level}");
        }

        [Button("Add 100 Gold & Save")]
        public void AddGold()
        {
            var save = DataSaveManager.Instance.Get<DemoPlayerSave>();
            save.Gold += 100;
            DataSaveManager.Instance.Save(save);
            Debug.Log($"[DataSave Demo] Gold is now {save.Gold}");
        }

        [Button("Level Up & Save")]
        public void LevelUp()
        {
            var save = DataSaveManager.Instance.Get<DemoPlayerSave>();
            save.Level++;
            DataSaveManager.Instance.Save(save);
            Debug.Log($"[DataSave Demo] Level is now {save.Level}");
        }

        [Button("Delete PlayerSave")]
        public void DeleteSave()
        {
            DataSaveManager.Instance.Delete<DemoPlayerSave>();
            Debug.Log("[DataSave Demo] PlayerSave deleted.");
        }

        [Button("Save All")]
        public void SaveAll()
        {
            DataSaveManager.Instance.SaveAll();
            Debug.Log("[DataSave Demo] All saves flushed.");
        }
    }

    // ── Demo Data ────────────────────────────────────────────────────────────

    [Serializable]
    public class DemoPlayerSave : BaseUserData
    {
        public string PlayerName = "Hero";
        public int Gold;
        public int Level = 1;
    }
}
