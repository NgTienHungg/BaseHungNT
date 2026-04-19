using UnityEngine;

namespace WingsMob.HungNT
{
    public class MonoSingletonScene<TMono> : MonoBehaviour where TMono : MonoBehaviour
    {
        private static TMono _instance;
        private static readonly object _lock = new object();

        public static TMono Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        _instance = FindObjectOfType<TMono>(includeInactive: true);

                        // create new instance
                        if (_instance == null)
                        {
                            var go = new GameObject(typeof(TMono).Name);
                            _instance = go.AddComponent<TMono>();
                        }
                    }
                }

                return _instance;
            }
        }

        private void Awake()
        {
            // khi có Instance sẵn trên scene
            if (_instance == null)
                _instance = this as TMono;

            if (_instance == this)
            {
                OnAwake();
                return;
            }

            this.LogWarning($"Destroy duplicate instance: {gameObject.name.Color("red")}");
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }

        protected virtual void OnAwake()
        {
            this.Log($"On awake of instance: {gameObject.name.Color("cyan")}");
        }
    }
}