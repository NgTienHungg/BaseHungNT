#if HUNGNT_TWEEN
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class UITweenRotate : UITweenBase
    {
        protected override string DefaultPresetResourcePath => "Tween/TweenPreset_Rotate";

        private float _originalAngle;

        public override void Init()
        {
            base.Init();

            _originalAngle = transform.localEulerAngles.z;
        }

        public override async UniTask Show(CancellationToken token = default)
        {
            await base.Show(token);
            await transform.DORotate(Vector3.forward * -360, DurationIn, RotateMode.FastBeyond360)
                .SetEase(EaseIn).SetDelay(DelayIn).SetLoops(-1, LoopType.Restart)
                .OnComplete(Active)
                .ToUniTask(cancellationToken: token);
        }

        public override async UniTask Hide(CancellationToken token = default)
        {
            await base.Hide(token);
        }

        public override void Active()
        {
            base.Active();
            transform.eulerAngles = new Vector3(0, 0, _originalAngle);
        }

        public override void Inactive()
        {
            base.Inactive();
            transform.eulerAngles = new Vector3(0, 0, _originalAngle);
        }
    }
}
#endif