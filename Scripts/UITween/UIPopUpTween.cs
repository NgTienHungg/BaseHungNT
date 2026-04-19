#if HUNGNT_TWEEN && HUNGNT_WINGSMOB
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using WingsMob.HungNT;

namespace WingsMob
{
    public class UIPopUpTween : UIPopUp
    {
        protected CancellationTokenSource _closeTweenCts;

        protected override void OnEnable()
        {
            base.OnEnable();
            _closeTweenCts?.Cancel();
            _closeTweenCts = new CancellationTokenSource();
        }

        public void CloseImmediately()
        {
            TrackingClosePopup();
            UIPopUpManager.Instance.DisablePopUp(gameObject);
        }

        public void PlaySfxButtonClicked()
        {
            SoundManager.Instance.PlayButtonSound();
            GameManager.Instance.OnTriggerVibration();
        }

        public override async void ClosePopUp(bool hasSound = true, bool closeByButtonBack = false)
        {
            if (hasSound)
            {
                PlaySfxButtonClicked();
            }

            _closeTweenCts?.Cancel(); // cancel last tween
            _closeTweenCts = new CancellationTokenSource();

            var uiTweens = GetComponentsInChildren<UITweenBase>()
                .Where(e => e.enabled).ToList(); // khong get cac component bi disable

            await UniTask.WhenAll(uiTweens.Select(tween => tween.Hide()).ToList())
                .AttachExternalCancellation(_closeTweenCts.Token)
                .SuppressCancellationThrow();

            TrackingClosePopup();
            UIPopUpManager.Instance.DisablePopUp(gameObject);
        }
    }
}
#endif