#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenMove : UITweenBase
    {
        [Title("Move")]
        [SerializeField] private Vector2 _offset;

        private RectTransform _rectTrans;
        private Vector2 _activeAnchorPos;
        private Vector2 _inactiveAnchorPos;

        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Move";

        public override void Init()
        {
            base.Init();

            if (_rectTrans == null)
            {
                _rectTrans = transform as RectTransform;
            }

            _activeAnchorPos = _rectTrans.anchoredPosition;
            _inactiveAnchorPos = _activeAnchorPos + _offset;
        }

        public override async UniTask Show(CancellationToken token = default)
        {
            await base.Show(token);
            await _rectTrans.DOAnchorPos(_activeAnchorPos, DurationIn)
                .SetEase(EaseIn).SetDelay(DelayIn)
                .OnComplete(Active)
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            await base.Hide(token);
            await _rectTrans.DOAnchorPos(_inactiveAnchorPos, DurationOut)
                .SetEase(EaseOut).SetDelay(DelayOut)
                .OnComplete(Inactive)
                .ToUniTask(cancellationToken: token);
        }

        public override void Active()
        {
            base.Active();
            _rectTrans.anchoredPosition = _activeAnchorPos;
        }

        public override void Inactive()
        {
            base.Inactive();
            _rectTrans.anchoredPosition = _inactiveAnchorPos;
        }
    }
}
#endif