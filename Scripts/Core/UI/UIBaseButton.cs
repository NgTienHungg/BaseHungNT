using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace WingsMob.HungNT
{
    [RequireComponent(typeof(Button))]
    public class UIBaseButton : UIBaseView
    {
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private RectTransform _content;
        [SerializeField] private TMP_Text _titleTxt;
        [SerializeField] private GameObject _highlightObj;

        private Action _buttonClicked;
        private Func<UniTask> _buttonClickedAsync;

        public void AddListener(Action action) => _buttonClicked += action;
        public void AddListenerAsync(Func<UniTask> asyncAction) => _buttonClickedAsync += asyncAction;
        public void RemoveListener(Action action) => _buttonClicked -= action;
        public void RemoveListenerAsync(Func<UniTask> asyncAction) => _buttonClickedAsync -= asyncAction;

        protected virtual void OnValidate()
        {
            if (_button == null)
                _button = GetComponent<Button>();
        }

        protected virtual void Awake()
        {
            _button.onClick.AddListener(OnActionClicked);
            SetHighlight(false);
        }

        public void SetSprite(Sprite sprite)
        {
            if (_image)
            {
                _image.sprite = sprite;
            }
        }

        public void SetTitle(string title)
        {
            if (_titleTxt)
            {
                _titleTxt.SetText(title);
            }
        }

        public void SetHighlight(bool active)
        {
            if (_highlightObj)
            {
                _highlightObj.SetActive(active);
            }
        }

        protected virtual void OnActionClicked()
        {
            _buttonClicked?.Invoke();

            if (_buttonClickedAsync != null)
            {
                UniTask.Void(async () =>
                {
                    var delegates = _buttonClickedAsync.GetInvocationList();
                    foreach (var d in delegates)
                    {
                        try
                        {
                            await ((Func<UniTask>)d).Invoke();
                        }
                        catch (Exception ex)
                        {
                            this.LogError($"[UIBaseButton] Async listener error: {ex}");
                        }
                    }
                });
            }
        }
    }
}