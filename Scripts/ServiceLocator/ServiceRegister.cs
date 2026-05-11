using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Đăng ký services vào <see cref="ServiceLocator"/> khi scene khởi động.
    /// — Mono services: tự động tìm qua <c>GetComponentsInChildren</c> trên cùng GameObject.
    /// — Non-Mono services: thêm trực tiếp từ Inspector qua <c>[SerializeReference]</c>.
    /// Đây là template per-project — override <see cref="OnRegisterServices"/> để thêm logic riêng.
    /// </summary>
    public class ServiceRegister : MonoBehaviour
    {
        [ShowInInspector, ReadOnly]
        [Tooltip("Danh sách MonoBehaviour services đã được register (chỉ đọc, cập nhật lúc Play Mode).")]
        private List<IService> _monoServices = new();

        [SerializeReference]
        [Tooltip("Plain C# services (non-MonoBehaviour). Thêm từ Inspector bằng cách right-click → 'Add Reference'.")]
        private List<IService> _nonMonoServices = new();

        // ── Unity ────────────────────────────────────────────────────────────

        private void Start()
        {
            Register();
        }

        protected virtual void Register()
        {
            RegisterMonoServices();
            RegisterNonMonoServices();
            OnRegisterServices();
        }

        // ── Overridable hook ─────────────────────────────────────────────────

        /// <summary>
        /// Override để đăng ký service tạo bằng code (SDK wrapper, factory, v.v.).
        /// </summary>
        protected virtual void OnRegisterServices() { }

        // ── Private ──────────────────────────────────────────────────────────

        private void RegisterMonoServices()
        {
            // Tìm tất cả MonoBehaviour implement IService trên GameObject và children
            var found = GetComponentsInChildren<IService>(includeInactive: true);

            _monoServices.Clear();

            foreach (var service in found)
            {
                RegisterByFirstServiceInterface(service);
                _monoServices.Add(service);
            }
        }

        private void RegisterNonMonoServices()
        {
            foreach (var service in _nonMonoServices)
            {
                if (service == null) continue;

                RegisterByFirstServiceInterface(service);
            }
        }

        private static void RegisterByFirstServiceInterface(IService impl)
        {
            var concreteType = impl.GetType();
            var keyType = FindFirstServiceInterface(concreteType) ?? concreteType;

            ServiceLocator.Instance.Register(keyType, impl);
        }

        /// <summary>
        /// Tìm interface con đầu tiên của <see cref="IService"/> trên <paramref name="type"/>.
        /// Trả về <c>null</c> nếu class implement <see cref="IService"/> trực tiếp (không có interface trung gian).
        /// </summary>
        private static Type FindFirstServiceInterface(Type type)
        {
            var iServiceType = typeof(IService);

            foreach (var iface in type.GetInterfaces())
            {
                if (iface != iServiceType && iServiceType.IsAssignableFrom(iface))
                    return iface;
            }

            return null;
        }
    }
}
