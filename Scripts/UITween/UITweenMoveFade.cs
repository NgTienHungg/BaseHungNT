#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenMoveFade : UITweenBase
    {
        [Title("Move")]
        [SerializeField] private Vector2 _offset;

        [Title("Fade")]
        [SerializeField] private float _inactiveAlpha = 0f;
        [SerializeField] private float _activeAlpha = 1f;

        private RectTransform _rectTrans;
        private Vector2 _activeAnchorPos;
        private Vector2 _inactiveAnchorPos;

        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Move";

        private CanvasGroup _canvasGroup;

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

            var sequence = DOTween.Sequence();
            sequence.Join(_rectTrans.DOAnchorPos(_activeAnchorPos, DurationIn).SetEase(EaseIn))
                .Join(_canvasGroup.DOFade(_activeAlpha, DurationIn).SetEase(EaseIn))
                .SetDelay(DelayIn)
                .OnComplete(Active);

            await sequence.ToUniTask(cancellationToken: token);
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            await base.Hide(token);

            var sequence = DOTween.Sequence();
            sequence.Join(_rectTrans.DOAnchorPos(_inactiveAnchorPos, DurationOut).SetEase(EaseOut))
                .Join(_canvasGroup.DOFade(_inactiveAlpha, DurationOut).SetEase(EaseOut))
                .SetDelay(DelayOut)
                .OnComplete(Inactive);

            await sequence.ToUniTask(cancellationToken: token);
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