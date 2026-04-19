#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenScale : UITweenBase
    {
        [Title("Scale")]
        [SerializeField] private Vector2 _inactiveScale = new Vector2(0f, 0f);
        [SerializeField] private Vector2 _activeScale = new Vector2(1f, 1f);

        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Scale";

        /// <summary>
        /// Sử dụng trực tiếp Vector2 cho các hàm DOScale đang bị bug crash unity nếu trong Popup có UIParticle
        /// Không rõ nguyên nhân, tạm thời dùng thông qua Vector3
        /// </summary>
        private Vector3 InactiveScale => new Vector3(_inactiveScale.x, _inactiveScale.y, 1f);
        private Vector3 ActiveScale => new Vector3(_activeScale.x, _activeScale.y, 1f);

        public override async UniTask Show(CancellationToken token = default)
        {
            await base.Show(token);
            await transform.DOScale(ActiveScale, DurationIn)
                .SetEase(EaseIn).SetDelay(DelayIn)
                .OnComplete(Active)
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            await base.Hide(token);
            await transform.DOScale(InactiveScale, DurationOut)
                .SetEase(EaseOut).SetDelay(DelayOut)
                .OnComplete(Inactive)
                .ToUniTask(cancellationToken: token);
        }

        public override void Active()
        {
            base.Active();
            transform.localScale = ActiveScale;
        }

        public override void Inactive()
        {
            base.Inactive();
            transform.localScale = InactiveScale;
        }
    }
}
#endif