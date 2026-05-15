using UnityEngine;

namespace HungNT
{
    [RequireComponent(typeof(RectTransform))]
    public class UIBaseView : MonoBehaviour
    {
        [SerializeField, HideInInspector] private RectTransform _rectTransform;
        [SerializeField, HideInInspector] private CanvasGroup _canvasGroup;
        [SerializeField, HideInInspector] private RectTransform _parent;

        public virtual RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = this.GetOrAddComponent<RectTransform>();

                return _rectTransform;
            }
        }

        public virtual CanvasGroup CanvasGroup
        {
            get
            {
                if (_canvasGroup == false)
                    _canvasGroup = this.GetOrAddComponent<CanvasGroup>();

                return _canvasGroup;
            }
        }

        public virtual RectTransform Parent
        {
            get
            {
                if (_parent == null)
                    _parent = RectTransform.parent as RectTransform;

                return _parent;
            }
        }

        public virtual bool Interactable
        {
            get
            {
                if (CanvasGroup)
                    return CanvasGroup.interactable;

                return true;
            }

            set
            {
                if (CanvasGroup)
                    CanvasGroup.interactable = value;
            }
        }

        public void PlaySfxButtonClicked()
        {
            // SoundManager.Instance.PlayButtonSound();
            // GameManager.Instance.OnTriggerVibration(WMVibrationType.Medium);
        }
    }
}