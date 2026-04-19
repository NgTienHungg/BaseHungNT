#if HUNGNT_SPINE
using Spine.Unity;
using UnityEngine;

namespace WingsMob.HungNT
{
    /// <summary>
    /// Tự động phát một animation của SkeletonGraphic sau một khoảng delay khi object được bật.
    /// SkeletonGraphic sẽ được tự động lấy từ GameObject hiện tại (yêu cầu phải có).
    /// </summary>
    [RequireComponent(typeof(SkeletonGraphic))]
    public class SkeletonAutoRun : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private string _animationName;
        [SerializeField] private bool _loop;
        [SerializeField] private float _delay = 0f;

        private SkeletonGraphic _skeleton;

        private void Awake()
        {
            _skeleton = GetComponent<SkeletonGraphic>();
        }

        private void OnEnable()
        {
            if (_skeleton == null || string.IsNullOrEmpty(_animationName))
            {
                WMLog.LogWarning("Skeleton hoặc animation name chưa được cấu hình.");
                return;
            }

            _skeleton.ResetAnim();
            _skeleton.enabled = false;
            Invoke(nameof(RunAnim), _delay);
        }

        /// <summary>
        /// Kích hoạt SkeletonGraphic và phát animation theo cấu hình.
        /// </summary>
        private void RunAnim()
        {
            _skeleton.enabled = true;
            _skeleton.AnimationState.SetAnimation(0, _animationName, _loop);
        }
    }
}
#endif