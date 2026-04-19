#if HUNGNT_TWEEN
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UINotifyManager : MonoSingleton<UINotifyManager>
    {
        [SerializeField] private UINotify _uiNotify;
        [SerializeField] private Transform _holder;
        [SerializeField] private float _notifyLifeTime = 5f;

        private void Start()
        {
            _uiNotify.gameObject.SetActive(false);
        }

        public void ShowNotify(string message)
        {
            var cloneNotify = Instantiate(_uiNotify, _holder);
            cloneNotify.ShowMessage(message);
            Destroy(cloneNotify.gameObject, _notifyLifeTime);
        }
    }
}
#endif