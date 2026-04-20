using UnityEngine;
using UnityEngine.UI;

namespace HungNT
{
    public abstract class AutoActionButton : MonoBehaviour
    {
        public Button button;

        protected virtual void Reset()
        {
            button = GetComponent<Button>();
        }

        protected virtual void OnEnable()
        {
            button.onClick.AddListener(OnClick);
        }

        protected virtual void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
        }

        protected abstract void OnClick();
    }
}