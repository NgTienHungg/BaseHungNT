#if HUNGNT_TWEEN
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UINotify : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private float _showDuration = 2f;

        public void ShowMessage(string message)
        {
            gameObject.SetActive(true);
            _messageText.text = message;
            Invoke(nameof(Hide), _showDuration);
        }

        public void Hide()
        {
            GetComponent<UITweenFade>().Hide().Forget();
        }
    }
}
#endif