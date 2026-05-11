using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Extension methods trên MonoBehaviour để gọi ServiceLocator ngắn gọn hơn.
    /// </summary>
    public static class ServiceLocatorExtensions
    {
        /// <summary>
        /// Lấy implementation của <typeparamref name="TService"/> từ ServiceLocator.
        /// </summary>
        public static TService GetService<TService>(this MonoBehaviour _) where TService : IService
        {
            return ServiceLocator.Instance.Get<TService>();
        }

        /// <summary>
        /// Thử lấy implementation của <typeparamref name="TService"/> từ ServiceLocator.
        /// Trả về true nếu tìm thấy.
        /// </summary>
        public static bool TryGetService<TService>(this MonoBehaviour _, out TService service) where TService : IService
        {
            return ServiceLocator.Instance.TryGet(out service);
        }

        /// <summary>
        /// Đăng ký implementation cho <typeparamref name="TService"/> vào ServiceLocator.
        /// </summary>
        public static void RegisterService<TService>(this MonoBehaviour _, TService impl) where TService : IService
        {
            ServiceLocator.Instance.Register(impl);
        }

        /// <summary>
        /// Hủy đăng ký <typeparamref name="TService"/> khỏi ServiceLocator.
        /// </summary>
        public static void UnregisterService<TService>(this MonoBehaviour _) where TService : IService
        {
            ServiceLocator.Instance.Unregister<TService>();
        }
    }
}