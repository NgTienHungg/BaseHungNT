using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Token để hủy đăng ký listener. Gọi <see cref="UnRegister"/> khi không cần lắng nghe nữa.
    /// </summary>
    public interface IUnRegister
    {
        void UnRegister();
    }

    /// <summary>
    /// Extension: tự động UnRegister khi GameObject bị Destroy.
    /// <code>
    /// hp.Register(OnHpChanged).UnRegisterOnDestroy(gameObject);
    /// </code>
    /// </summary>
    public static class UnRegisterExtensions
    {
        public static void UnRegisterOnDestroy(this IUnRegister unRegister, GameObject go)
        {
            if (go == null) return;

            var trigger = go.GetComponent<UnRegisterOnDestroyTrigger>();
            if (trigger == null)
                trigger = go.AddComponent<UnRegisterOnDestroyTrigger>();

            trigger.Add(unRegister);
        }

        public static void UnRegisterOnDestroy(this IUnRegister unRegister, MonoBehaviour mono)
        {
            UnRegisterOnDestroy(unRegister, mono.gameObject);
        }
    }

    /// <summary>
    /// Helper component — tự gọi UnRegister trên tất cả token khi bị Destroy.
    /// </summary>
    [DisallowMultipleComponent]
    internal class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly System.Collections.Generic.List<IUnRegister> _tokens = new();

        public void Add(IUnRegister token) => _tokens.Add(token);

        private void OnDestroy()
        {
            foreach (var token in _tokens)
                token.UnRegister();
            _tokens.Clear();
        }
    }
}
