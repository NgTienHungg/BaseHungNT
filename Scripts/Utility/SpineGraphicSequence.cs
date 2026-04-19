#if HUNGNT_SPINE
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace WingsMob.HungNT
{
    public class SpineGraphicSequence : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic _skeletonAnimation;
        [SerializeField] private bool _playOnEnable = true;
        [SerializeField] private bool _reloadOnStart = true;
        [SerializeField] private float _delayStart;
        [SerializeField] private List<SpineAnimTracker> _trackers = new();

        private void OnValidate()
        {
            if (_skeletonAnimation == null)
            {
                _skeletonAnimation = GetComponentInChildren<SkeletonGraphic>();
            }
        }

        private void OnEnable()
        {
            if (_playOnEnable)
            {
                RunAnimSequence().Forget();
            }
        }

        public void RunAnimSequenceForget()
        {
            RunAnimSequence().Forget();
        }

        public async UniTask RunAnimSequence()
        {
            if (_skeletonAnimation == null)
                return;

            await UniTask.WaitForSeconds(_delayStart, cancellationToken: destroyCancellationToken);

            if (_reloadOnStart) _skeletonAnimation.Skeleton.SetToSetupPose();
            _skeletonAnimation.AnimationState.ClearTracks();
            _skeletonAnimation.AnimationState.SetAnimation(0, _trackers[0].animation, _trackers[0].loop);

            for (var i = 1; i < _trackers.Count; i++)
            {
                _skeletonAnimation.AnimationState.AddAnimation(0, _trackers[i].animation, _trackers[i].loop, _trackers[i].delay);
            }
        }

        [Button(ButtonSizes.Large)]
        private void SetAnimationImmediate()
        {
            if (_skeletonAnimation == null || _trackers.Count == 0)
                return;

            if (_reloadOnStart) _skeletonAnimation.Skeleton.SetToSetupPose();
            _skeletonAnimation.AnimationState.ClearTracks();
            _skeletonAnimation.AnimationState.SetAnimation(0, _trackers[0].animation, _trackers[0].loop);
        }
    }
}
#endif