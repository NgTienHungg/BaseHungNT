using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Implementation của <see cref="IDatasaveService"/> — dùng EasySave3 (ES3) làm backend.
    /// <para>Gắn lên cùng GameObject với <see cref="T:HungNT.ServiceRegister"/> để tự đăng ký.</para>
    /// </summary>
    public class DataSaveService : MonoBehaviour, IDatasaveService
    {
        [SerializeField, Tooltip("Tên file ES3 lưu trữ. Mặc định: GameSave.es3")]
        private string _saveFileName = "GameSave.es3";

        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Type, BaseUserData> _cache = new();

        private ES3Settings _es3Settings;

        // ── IService ─────────────────────────────────────────────────────────

        public void Initialize()
        {
            _es3Settings = new ES3Settings(_saveFileName);
            this.Log($"Initialized. File: {_saveFileName}");
        }

        public void LateInitialize() { }

        // ── IDatasaveService ─────────────────────────────────────────────────

        public T GetData<T>() where T : BaseUserData, new()
        {
            var type = typeof(T);

            if (_cache.TryGetValue(type, out var cached))
                return (T)cached;

            var data = new T();
            data = ES3.KeyExists(data.Key, _es3Settings)
                ? ES3.Load<T>(data.Key, _es3Settings)
                : data;

            data.OnAfterLoad();
            _cache[type] = data;

            this.Log($"Loaded {type.Name} (key={data.Key})");
            return data;
        }

        public void Save<T>(T data) where T : BaseUserData
        {
            ES3.Save(data.Key, data, _es3Settings);
            _cache[data.GetType()] = data;
            this.Log($"Saved {data.GetType().Name}");
        }

        public void Save<T>() where T : BaseUserData, new()
        {
            Save(GetData<T>());
        }

        public void SaveAll()
        {
            foreach (var kvp in _cache)
            {
                ES3.Save(kvp.Value.Key, kvp.Value, _es3Settings);
            }
            this.Log($"SaveAll: {_cache.Count} entries flushed.");
        }

        public void Delete<T>() where T : BaseUserData, new()
        {
            var type = typeof(T);
            string key;

            if (_cache.TryGetValue(type, out var cached))
            {
                key = cached.Key;
                _cache.Remove(type);
            }
            else
            {
                key = new T().Key;
            }

            if (ES3.KeyExists(key, _es3Settings))
                ES3.DeleteKey(key, _es3Settings);

            this.Log($"Deleted {type.Name} (key={key})");
        }

        public void DeleteAll()
        {
            ES3.DeleteFile(_es3Settings);
            _cache.Clear();
            this.Log("DeleteAll: save file removed.");
        }

        // ── Unity ────────────────────────────────────────────────────────────

        private void OnApplicationPause(bool pause)
        {
            if (pause) SaveAll();
        }

        private void OnApplicationQuit()
        {
            SaveAll();
        }
    }
}
