using System;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Facade quản lý toàn bộ ads. Delegate xuống các ad providers theo SDK.
    /// Xử lý cooldown giữa các lần show, skip ads, auto-preload.
    /// </summary>
    public class AdsService : MonoBehaviour, IAdsService
    {
        [SerializeField] private bool _isSkipAds;

        // ── Providers ────────────────────────────────────────────────────────

        private IAppOpenAdProvider _appOpenProvider = new NullAppOpenAdProvider();
        private IBannerAdProvider _bannerProvider = new NullBannerAdProvider();
        private IInterstitialAdProvider _interProvider = new NullInterstitialAdProvider();
        private IRewardedAdProvider _rewardedProvider = new NullRewardedAdProvider();

        // ── Timing ───────────────────────────────────────────────────────────

        private float _lastInterTime = -999f;
        private float _lastRewardTime = -999f;

        // ══════════════════════════════════════════════════════════════════════
        //  IService
        // ══════════════════════════════════════════════════════════════════════

        public void Initialize()
        {
            var config = Resources.Load<AdsConfig>(AdsDefine.ADS_CONFIG_PATH);
            if (config == null)
            {
                this.LogError($"AdsConfig not found at Resources/{AdsDefine.ADS_CONFIG_PATH}");
                return;
            }

            InitializeProviders(config);
        }

        public void LateInitialize()
        {
        }

        private void InitializeProviders(AdsConfig config)
        {
            _appOpenProvider = new NullAppOpenAdProvider();
            _bannerProvider = new NullBannerAdProvider();
            _interProvider = new NullInterstitialAdProvider();
            _rewardedProvider = new NullRewardedAdProvider();

#if USE_ADMOB
            _appOpenProvider = new AdMobAppOpenProvider();
#endif

#if USE_MAX
            _bannerProvider = new MaxBannerProvider();
            _interProvider = new MaxInterstitialProvider();
            _rewardedProvider = new MaxRewardedProvider();
#elif USE_IRONSOURCE
            _bannerProvider = new IronSourceBannerProvider();
            _interProvider = new IronSourceInterstitialProvider();
            _rewardedProvider = new IronSourceRewardedProvider();
#elif USE_ADMOB
            _bannerProvider = new AdMobBannerProvider();
            _interProvider = new AdMobInterstitialProvider();
            _rewardedProvider = new AdMobRewardedProvider();
#endif

            _appOpenProvider.Initialize(config);
            _bannerProvider.Initialize(config);
            _interProvider.Initialize(config);
            _rewardedProvider.Initialize(config);

            this.Log("AdsService initialized.");
        }

        private void OnDestroy()
        {
            _appOpenProvider.Dispose();
            _bannerProvider.Dispose();
            _interProvider.Dispose();
            _rewardedProvider.Dispose();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Banner
        // ══════════════════════════════════════════════════════════════════════

        public void ShowBanner()
        {
            if (_isSkipAds) return;
            _bannerProvider.ShowBanner();
        }

        public void HideBanner()
        {
            _bannerProvider.HideBanner();
        }

        // ══════════════════════════════════════════════════════════════════════
        //  App Open
        // ══════════════════════════════════════════════════════════════════════

        public void ShowAppOpen(Action onComplete = null)
        {
            if (_isSkipAds)
            {
                onComplete?.Invoke();
                return;
            }

            if (!_appOpenProvider.IsAdReady())
            {
                onComplete?.Invoke();
                return;
            }

            _appOpenProvider.ShowAd(onComplete);
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Interstitial
        // ══════════════════════════════════════════════════════════════════════

        public bool IsInterstitialReady()
        {
            return _interProvider.IsAdReady();
        }

        public void ShowInterstitial(
            string placement = AdsPlacement.DEFAULT,
            Action onSuccess = null,
            Action onFailure = null)
        {
            if (_isSkipAds)
            {
                onSuccess?.Invoke();
                return;
            }

            if (!CanShowInterstitial())
            {
                onFailure?.Invoke();
                return;
            }

            if (!_interProvider.IsAdReady())
            {
                onFailure?.Invoke();
                _interProvider.LoadAd();
                return;
            }

            this.Log($"ShowInterstitial — placement: {placement}");

            _interProvider.ShowAd(
                onSuccess: () =>
                {
                    _lastInterTime = Time.realtimeSinceStartup;
                    onSuccess?.Invoke();
                },
                onFailure: () =>
                {
                    onFailure?.Invoke();
                });
        }

        private bool CanShowInterstitial()
        {
            float now = Time.realtimeSinceStartup;
            float elapsedSinceInter = now - _lastInterTime;
            float elapsedSinceReward = now - _lastRewardTime;

            return elapsedSinceInter >= AdsDefine.INTER_COOLDOWN
                   && elapsedSinceReward >= AdsDefine.REWARD_TO_INTER_COOLDOWN;
        }

        // ══════════════════════════════════════════════════════════════════════
        //  Rewarded
        // ══════════════════════════════════════════════════════════════════════

        public bool IsRewardedReady()
        {
            return _rewardedProvider.IsAdReady();
        }

        public void ShowRewarded(
            string placement,
            Action onSuccess,
            Action onFailure = null)
        {
            if (_isSkipAds)
            {
                onSuccess?.Invoke();
                return;
            }

            if (!_rewardedProvider.IsAdReady())
            {
                onFailure?.Invoke();
                _rewardedProvider.LoadAd();
                return;
            }

            this.Log($"ShowRewarded — placement: {placement}");

            _rewardedProvider.ShowAd(
                onSuccess: () =>
                {
                    _lastRewardTime = Time.realtimeSinceStartup;
                    onSuccess?.Invoke();
                },
                onFailure: () =>
                {
                    onFailure?.Invoke();
                });
        }
    }
}