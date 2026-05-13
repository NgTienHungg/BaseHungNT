using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Singleton quản lý tất cả <see cref="IDataTable"/>.
    /// <para>Đăng ký table qua <see cref="Register"/> hoặc <see cref="RegisterAll"/>.</para>
    /// <para>Truy vấn table qua <see cref="GetTable{T}"/>.</para>
    /// </summary>
    public class DatabaseManager : MonoSingleton<DatabaseManager>
    {
        [SerializeField, Tooltip("Kéo thả các ScriptableObject (BaseDataTable) vào đây để auto-register.")]
        private BaseDataTable[] _tables;

        [ShowInInspector, ReadOnly]
        private readonly Dictionary<Type, IDataTable> _registry = new();

        protected override void OnAwake()
        {
            base.OnAwake();

            if (_tables != null && _tables.Length > 0)
                RegisterAll(_tables);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _registry.Clear();
        }

        // ── Register ─────────────────────────────────────────────────────────

        /// <summary>Đăng ký 1 table vào registry.</summary>
        public void Register(IDataTable table)
        {
            if (table == null) return;

            var type = table.GetType();
            if (_registry.ContainsKey(type))
            {
                this.LogWarning($"Table {type.Name} already registered — overwriting.");
            }

            _registry[type] = table;
            table.Initialize();
            this.Log($"Registered table: {type.Name}");
        }

        /// <summary>Đăng ký nhiều table cùng lúc.</summary>
        public void RegisterAll(IEnumerable<BaseDataTable> tables)
        {
            foreach (var table in tables)
                Register(table);
        }

        // ── Query ────────────────────────────────────────────────────────────

        /// <summary>Lấy table theo type. Trả về null nếu chưa register.</summary>
        public T GetTable<T>() where T : class, IDataTable
        {
            var type = typeof(T);
            if (_registry.TryGetValue(type, out var table))
                return (T)table;

            this.LogWarning($"GetTable<{type.Name}> — table chưa register.");
            return null;
        }

        /// <summary>Thử lấy table. Trả về true nếu tìm thấy.</summary>
        public bool TryGetTable<T>(out T table) where T : class, IDataTable
        {
            if (_registry.TryGetValue(typeof(T), out var found))
            {
                table = (T)found;
                return true;
            }

            table = null;
            return false;
        }

        /// <summary>Kiểm tra table đã register chưa.</summary>
        public bool HasTable<T>() where T : class, IDataTable
        {
            return _registry.ContainsKey(typeof(T));
        }

        // ── Debug ────────────────────────────────────────────────────────────

        public IReadOnlyDictionary<Type, IDataTable> GetDebugSnapshot() => _registry;
    }
}
