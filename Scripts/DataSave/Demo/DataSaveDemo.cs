using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo IDatasaveService — gắn lên cùng scene với DataSaveService và ServiceRegister.
    /// </summary>
    public class DataSaveDemo : MonoBehaviour
    {
        // ── Runtime state (Odin Inspector) ───────────────────────────────────

        [ShowInInspector, ReadOnly, FoldoutGroup("Coin")]
        private CoinSave _coin;

        [ShowInInspector, ReadOnly, FoldoutGroup("Level")]
        private LevelSave _level;

        [ShowInInspector, ReadOnly, FoldoutGroup("Profile")]
        private UserProfileSave _profile;

        [ShowInInspector, ReadOnly, FoldoutGroup("Settings")]
        private SettingsSave _settings;

        [ShowInInspector, ReadOnly, FoldoutGroup("Unlock")]
        private UnlockSave _unlock;

        private IDatasaveService _saveService;

        // ── Unity ────────────────────────────────────────────────────────────

        private void Start()
        {
            _saveService = this.GetService<IDatasaveService>();
            RefreshDisplay();
        }

        // ── Demo Buttons ─────────────────────────────────────────────────────

        [Button("Load All"), FoldoutGroup("Actions")]
        private void RefreshDisplay()
        {
            _coin     = _saveService.GetData<CoinSave>();
            _level    = _saveService.GetData<LevelSave>();
            _profile  = _saveService.GetData<UserProfileSave>();
            _settings = _saveService.GetData<SettingsSave>();
            _unlock   = _saveService.GetData<UnlockSave>();
        }

        [Button("+100 Coin"), FoldoutGroup("Actions")]
        private void AddCoin()
        {
            var data = _saveService.GetData<CoinSave>();
            data.Amount += 100;
            _saveService.Save(data);
            _coin = data;
        }

        [Button("Next Level"), FoldoutGroup("Actions")]
        private void NextLevel()
        {
            var data = _saveService.GetData<LevelSave>();
            data.SetStars(data.CurrentLevel, 3);
            data.CurrentLevel++;
            data.MaxUnlockedLevel = data.CurrentLevel;
            _saveService.Save(data);
            _level = data;
        }

        [Button("Change Username"), FoldoutGroup("Actions")]
        private void ChangeUsername()
        {
            var data = _saveService.GetData<UserProfileSave>();
            data.Username = $"Player_{Random.Range(1000, 9999)}";
            _saveService.Save(data);
            _profile = data;
        }

        [Button("Toggle Vibration"), FoldoutGroup("Actions")]
        private void ToggleVibration()
        {
            var data = _saveService.GetData<SettingsSave>();
            data.Vibration = !data.Vibration;
            _saveService.Save(data);
            _settings = data;
        }

        [Button("Unlock Item_001"), FoldoutGroup("Actions")]
        private void UnlockItem()
        {
            var data = _saveService.GetData<UnlockSave>();
            data.Unlock("Item_001");
            _saveService.Save(data);
            _unlock = data;
        }

        [Button("Save All"), FoldoutGroup("Actions")]
        private void SaveAll() => _saveService.SaveAll();

        [Button("DELETE ALL SAVE DATA"), FoldoutGroup("Actions")]
        private void DeleteAll()
        {
            _saveService.DeleteAll();
            RefreshDisplay();
        }
    }
}
