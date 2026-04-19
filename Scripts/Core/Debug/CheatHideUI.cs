using UnityEngine;

namespace WingsMob.HungNT
{
    public class CheatHideUI : MonoBehaviour
    {
        [SerializeField] private CanvasGroup[] uiElementsToHide;

        private bool isHidden = false;

        private void Awake()
        {
            var button = GetComponentInChildren<UnityEngine.UI.Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            var targetAlpha = isHidden ? 1f : 0f;
            isHidden = !isHidden;

            foreach (var uiElement in uiElementsToHide)
            {
                uiElement.alpha = targetAlpha;
            }
        }
    }
}