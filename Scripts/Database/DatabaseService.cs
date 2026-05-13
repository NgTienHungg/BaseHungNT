using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Implementation của <see cref="IDatabaseService"/>.
    /// Load tất cả <see cref="BaseDataTable"/> ScriptableObject từ <c>Resources/Database/</c>
    /// rồi Initialize và đăng ký vào registry.
    /// <para>Gắn lên cùng GameObject với <see cref="ServiceRegister"/> để tự đăng ký.</para>
    /// </summary>
    public class DatabaseService : MonoBehaviour, IDatabaseService
    {
        private const string ResourcesPath = "Database";

        [ShowInInspector, ReadOnly, DictionaryDrawerSettings(IsReadOnly = true)]
        private readonly Dictionary<Type, BaseDataTable> _registry = new();

        // ── IService ─────────────────────────────────────────────────────────

        public void Initialize()
        {
            LoadAllFromResources();
        }

        public void LateInitialize() { }

        // ── IDatabaseService ─────────────────────────────────────────────────

        public T GetTable<T>() where T : BaseDataTable
        {
            var type = typeof(T);
            if (_registry.TryGetValue(type, out var table))
                return (T)table;

            throw new InvalidOperationException(
                $"[DatabaseService] Table '{type.Name}' not found. " +
                $"Make sure a ScriptableObject of this type is placed under Resources/{ResourcesPath}/");
        }

        public bool TryGetTable<T>(out T table) where T : BaseDataTable
        {
            if (_registry.TryGetValue(typeof(T), out var raw))
            {
                table = (T)raw;
                return true;
            }
            table = null;
            return false;
        }

        public bool HasTable<T>() where T : BaseDataTable =>
            _registry.ContainsKey(typeof(T));

        // ── Internal ─────────────────────────────────────────────────────────

        private void LoadAllFromResources()
        {
            var assets = Resources.LoadAll<BaseDataTable>(ResourcesPath);

            if (assets == null || assets.Length == 0)
            {
                this.Log($"No BaseDataTable assets found at Resources/{ResourcesPath}/");
                return;
            }

            foreach (var asset in assets)
            {
                var type = asset.GetType();
                if (_registry.ContainsKey(type))
                {
                    this.Log($"Duplicate table type '{type.Name}' — skipping second asset '{asset.name}'.");
                    continue;
                }

                asset.Initialize();
                _registry[type] = asset;
                this.Log($"Loaded table: {type.Name} ({asset.name})");
            }

            this.Log($"Database ready. {_registry.Count} table(s) loaded.");
        }

#if UNITY_EDITOR
        [Button("Reload (Editor Only)"), PropertyOrder(-1)]
        private void ReloadInEditor()
        {
            _registry.Clear();
            LoadAllFromResources();
        }
#endif
    }
}
