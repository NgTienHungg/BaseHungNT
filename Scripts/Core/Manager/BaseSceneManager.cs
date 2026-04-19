using Cysharp.Threading.Tasks;
using UnityEngine;

namespace WingsMob.HungNT
{
    public abstract class BaseSceneManager<TMono> : MonoSingletonScene<TMono> where TMono : MonoBehaviour
    {
        [SerializeField] private bool _initOnStart = true;

        public virtual bool IsInitialized { get; protected set; }

        protected virtual void Start()
        {
            if (_initOnStart)
            {
                Initialize().ContinueWith(() =>
                {
                    IsInitialized = true;
                    OnEnterScene();
                }).Forget();
            }
        }

        public virtual UniTask Initialize()
        {
            this.Log("Initialize...".Color("orange"));
            return UniTask.CompletedTask;
        }

        public virtual UniTask OnEnterScene()
        {
            this.Log("Star Scene...".Color("lime"));
            return UniTask.CompletedTask;
        }
    }
}