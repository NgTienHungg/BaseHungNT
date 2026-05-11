using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Null Object implementation của IAdsService.
    /// Dùng làm fallback mặc định khi chưa có SDK thật,
    /// hoặc trong môi trường Editor / test không có ads.
    /// </summary>
    public class NullAdsService : MonoBehaviour, IAdsService
    {
        public void ShowBanner()
        {
        }

        public void HideBanner()
        {
        }

        public bool IsInterstitialReady()
        {
            return false;
        }

        public void ShowInterstitial(Action onComplete)
        {
            onComplete?.Invoke();
        }

        public bool IsRewardedReady()
        {
            return false;
        }

        public void ShowRewarded(Action<bool> onComplete)
        {
            // Null implementation không hiển thị ads, trả về false (không nhận reward)
            onComplete?.Invoke(false);
        }
    }
}
