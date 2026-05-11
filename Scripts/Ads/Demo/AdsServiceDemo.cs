using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo cách sử dụng AdsService thông qua ServiceLocator.
    /// Gán vào một GameObject, chạy Play Mode, nhấn các nút trong Inspector.
    /// </summary>
    public class AdsServiceDemo : MonoBehaviour
    {
        private IAdsService _ads;

        private void Start()
        {
            this.GetService<IAdsService>(ads => _ads = ads);
        }

        // ── Banner ───────────────────────────────────────────────────────────

        [Button]
        public void ShowBanner()
        {
            _ads.ShowBanner();
            this.Log("ShowBanner called.");
        }

        [Button]
        public void HideBanner()
        {
            _ads.HideBanner();
            this.Log("HideBanner called.");
        }

        // ── App Open ─────────────────────────────────────────────────────────

        [Button]
        public void ShowAppOpen()
        {
            _ads.ShowAppOpen(onComplete: () =>
            {
                this.Log("AppOpen completed.");
            });
        }

        // ── Interstitial ─────────────────────────────────────────────────────

        [Button]
        public void CheckInterstitialReady()
        {
            this.Log($"IsInterstitialReady: {_ads.IsInterstitialReady()}");
        }

        [Button]
        public void ShowInterstitialDefault()
        {
            _ads.ShowInterstitial(
                placement: AdsPlacement.DEFAULT,
                onSuccess: () => this.Log("Interstitial — success."),
                onFailure: () => this.Log("Interstitial — failure (cooldown / not ready).")
            );
        }

        [Button]
        public void ShowInterstitialLevelComplete()
        {
            _ads.ShowInterstitial(
                placement: AdsPlacement.LEVEL_COMPLETE,
                onSuccess: () => this.Log("Interstitial level_complete — success."),
                onFailure: () => this.Log("Interstitial level_complete — failure.")
            );
        }

        // ── Rewarded ─────────────────────────────────────────────────────────

        [Button]
        public void CheckRewardedReady()
        {
            this.Log($"IsRewardedReady: {_ads.IsRewardedReady()}");
        }

        [Button]
        public void ShowRewardedDoubleCoin()
        {
            _ads.ShowRewarded(
                placement: AdsPlacement.DOUBLE_COIN,
                onSuccess: () =>
                {
                    this.Log("Rewarded double_coin — user earned reward!");
                    // GiveReward();
                },
                onFailure: () =>
                {
                    this.Log("Rewarded double_coin — failed or closed early.");
                }
            );
        }

        [Button]
        public void ShowRewardedExtraLife()
        {
            _ads.ShowRewarded(
                placement: AdsPlacement.EXTRA_LIFE,
                onSuccess: () =>
                {
                    this.Log("Rewarded extra_life — user earned reward!");
                    // GiveExtraLife();
                },
                onFailure: () =>
                {
                    this.Log("Rewarded extra_life — failed or closed early.");
                }
            );
        }
    }
}