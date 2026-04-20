using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace HungNT
{
    public class UnregisterUniTask : MonoBehaviour
    {
        private Stack<CancellationTokenSource> cts = new();
        public Stack<CancellationTokenSource> CTS => cts;

        public CancellationTokenSource Register()
        {
            var t = new CancellationTokenSource();
            var combine = CancellationTokenSource.CreateLinkedTokenSource(t.Token, destroyCancellationToken);
            cts.Push(combine);
            return combine;
        }

        private void OnDisable()
        {
            while (cts.TryPop(out var t))
            {
                t.Cancel();
            }
        }
    }

    public static partial class UniTaskExtension
    {
        public static UniTask Unregister(this UniTask disposable, MonoBehaviour gameObject)
        {
            var reg = gameObject.GetOrAddComponent<UnregisterUniTask>();
            if (reg != null)
            {
                return disposable.AttachExternalCancellation(reg.Register().Token);
            }
            return disposable;
        }

        public static UniTask<T> Unregister<T>(this UniTask<T> disposable, MonoBehaviour gameObject)
        {
            var reg = gameObject.GetOrAddComponent<UnregisterUniTask>();
            if (reg != null)
            {
                return disposable.AttachExternalCancellation(reg.Register().Token);
            }
            return disposable;
        }

        public static CancellationToken UnregisterCancellationToken(this MonoBehaviour gameObject)
        {
            var reg = gameObject.GetOrAddComponent<UnregisterUniTask>();
            if (reg != null)
            {
                return reg.Register().Token;
            }
            return default;
        }
    }
}