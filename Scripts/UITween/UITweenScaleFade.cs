#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenScaleFade : UITweenBase
    {
        [Title("Scale")]
        [SerializeField] private Vector2 _inactiveScale = new Vector2(0f, 0f);
        [SerializeField] private Vector2 _activeScale = new Vector2(1f, 1f);

        [Title("Fade")]
        [SerializeField] private float _inactiveAlpha = 0f;
        [SerializeField] private float _activeAlpha = 1f;

        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Scale";

        /// <summary>
        /// Sử dụng trực tiếp Vector2 cho các hàm DOScale đang bị bug crash unity nếu trong Popup có UIParticle
        /// Không rõ nguyên nhân, tạm thời dùng thông qua Vector3
        /// </summary>
        private Vector3 InactiveScale => new Vector3(_inactiveScale.x, _inactiveScale.y, 1f);
        private Vector3 ActiveScale => new Vector3(_activeScale.x, _activeScale.y, 1f);

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
        }

        public override async UniTask Show(CancellationToken token = default)
        {
            await base.Show(token);

            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(ActiveScale, DurationIn).SetEase(EaseIn))
                .Join(_canvasGroup.DOFade(_activeAlpha, DurationIn).SetEase(EaseIn))
                .SetDelay(DelayIn)
                .OnComplete(Active);

            await sequence.ToUniTask(cancellationToken: token);
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            await base.Hide(token);

            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(InactiveScale, DurationOut).SetEase(EaseOut))
                .Join(_canvasGroup.DOFade(_inactiveAlpha, DurationOut).SetEase(EaseOut))
                .SetDelay(DelayOut)
                .OnComplete(Inactive);

            await sequence.ToUniTask(cancellationToken: token);
        }

        public override void Active()
        {
            base.Active();
            _canvasGroup.alpha = _activeAlpha;
            _canvasGroup.interactable = true;
            transform.localScale = ActiveScale;
        }

        public override void Inactive()
        {
            base.Inactive();
            _canvasGroup.alpha = _inactiveAlpha;
            _canvasGroup.interactable = false;
            transform.localScale = InactiveScale;
        }
    }
}
#endif