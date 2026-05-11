using System;

namespace HungNT
{
    /// <summary>
    /// Interface cho Ads service.
    /// Implement bằng SDK cụ thể (AdMob, IronSource, MAX, ...).
    /// </summary>
    public interface IAdsService : IService
    {
        // ── Banner ───────────────────────────────────────────────────────────

        void ShowBanner();

        void HideBanner();

        // ── Interstitial ─────────────────────────────────────────────────────

        bool IsInterstitialReady();

        /// <param name="onComplete">Callback khi quảng cáo đóng lại.</param>
        void ShowInterstitial(Action onComplete);

        // ── Rewarded ─────────────────────────────────────────────────────────

        bool IsRewardedReady();

        /// <param name="onComplete">Callback với kết quả: true nếu xem đủ và nhận reward.</param>
        void ShowRewarded(Action<bool> onComplete);
    }
}
