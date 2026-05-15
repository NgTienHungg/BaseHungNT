using HungNT;
﻿#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenFade : UITweenBase
    {
        [Title("Fade")]
        [SerializeField] private float _inactiveAlpha = 0f;
        [SerializeField] private float _activeAlpha = 1f;

        private CanvasGroup _canvasGroup;

        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Fade";

        public override void Init()
        {
            base.Init();

            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();

                if (_canvasGroup == null)
                {
                    _canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
        }

        public override async UniTask Show(CancellationToken token = default)
        {
            await base.Show(token);
            await _canvasGroup.DOFade(_activeAlpha, DurationIn)
                .SetEase(EaseIn).SetDelay(DelayIn)
                .OnComplete(Active)
                .ToUniTask(cancellationToken: token);

            //todo: .OnComplete(Active) là gắn action vào tween, khi task bị cancel, action này sẽ không được gọi
            //todo: .ContinueWithTask(Active) là gắn action vào sau task, khi task bị cancel thì chạy tiếp tới action
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            _canvasGroup.interactable = false;

            await base.Hide(token);
            await _canvasGroup.DOFade(_inactiveAlpha, DurationOut)
                .SetEase(EaseOut).SetDelay(DelayOut)
                .OnComplete(Inactive)
                .ToUniTask(cancellationToken: token);
        }

        public override void Active()
        {
            try
            {
                base.Active();
                _canvasGroup.alpha = _activeAlpha;
                _canvasGroup.interactable = true;
            }
            catch (System.Exception e)
            {
                WMLog.LogError("Active error: ".Color("red") + e.Message);
            }
        }

        public override void Inactive()
        {
            try
            {
                base.Inactive();
                _canvasGroup.alpha = _inactiveAlpha;
                _canvasGroup.interactable = false;
            }
            catch (System.Exception e)
            {
                WMLog.LogError("Inactive error: ".Color("red") + e.Message);
            }
        }
    }
}
#endif