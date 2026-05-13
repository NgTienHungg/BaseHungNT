using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

namespace HungNT
{
    /// <summary>
    /// Singleton quản lý save/load tất cả <see cref="BaseUserData"/>.
    /// <para>Mặc định dùng <see cref="PlayerPrefsDataSave"/>.</para>
    /// <para>Gọi <see cref="SetBackend"/> để thay đổi backend (VD: BayatGames, EasySave).</para>
    /// </summary>
    public class DataSaveManager : MonoSingleton<DataSaveManager>
    {
        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Type, BaseUserData> _cache = new();

        private IDataSave _backend;

        public IDataSave Backend => _backend;

        protected override void OnAwake()
        {
            base.OnAwake();
            _backend = new PlayerPrefsDataSave();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _cache.Clear();
        }

        // ── Configuration ────────────────────────────────────────────────────

        /// <summary>Thay đổi backend save/load (VD: BayatGames, EasySave, custom...).</summary>
        public void SetBackend(IDataSave backend)
        {
            _backend = backend ?? new PlayerPrefsDataSave();
            this.Log($"Backend changed to {_backend.GetType().Name}.");
        }

        // ── Get / Save / Delete ──────────────────────────────────────────────

        /// <summary>
        /// Lấy data. Nếu chưa load sẽ tự load từ backend.
        /// Data được cache lại — gọi nhiều lần không load lại.
        /// </summary>
        public T Get<T>() where T : BaseUserData, new()
        {
            var type = typeof(T);

            if (_cache.TryGetValue(type, out var cached))
                return (T)cached;

            var data = new T();
            var loaded = _backend.Load(data.Key, data);
            loaded.OnAfterLoad();

            _cache[type] = loaded;
            return loaded;
        }

        /// <summary>Lưu data xuống backend.</summary>
        public void Save<T>(T data) where T : BaseUserData
        {
            _backend.Save(data.Key, data);
        }

        /// <summary>Lưu data đã cache theo type.</summary>
        public void Save<T>() where T : BaseUserData, new()
        {
            var data = Get<T>();
            _backend.Save(data.Key, data);
        }

        /// <summary>Lưu tất cả data đang cache.</summary>
        public void SaveAll()
        {
            foreach (var kvp in _cache)
                _backend.Save(kvp.Value.Key, kvp.Value);
        }

        /// <summary>Xóa data theo type khỏi cache và backend.</summary>
        public void Delete<T>() where T : BaseUserData, new()
        {
            var type = typeof(T);
            if (_cache.TryGetValue(type, out var data))
            {
                _backend.Delete(data.Key);
                _cache.Remove(type);
            }
            else
            {
                var temp = new T();
                _backend.Delete(temp.Key);
            }
        }

        /// <summary>Xóa toàn bộ cache (không xóa từ backend).</summary>
        public void ClearCache()
        {
            _cache.Clear();
        }
    }
}
