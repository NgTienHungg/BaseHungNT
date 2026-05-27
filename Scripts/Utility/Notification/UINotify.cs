using Cysharp.Threading.Tasks;
using HungNT.UI.Tween;
using TMPro;
using UnityEngine;

namespace HungNT.UI.Tween
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
