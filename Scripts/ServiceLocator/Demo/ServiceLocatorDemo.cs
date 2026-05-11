using System.Collections.Generic;
using UnityEngine;

namespace HungNT.Demo
{
    /// <summary>
    /// Demo: Cách sử dụng ServiceLocator.
    /// Gán vào một GameObject và nhấn các nút trong Inspector (Play Mode).
    /// </summary>
    public class ServiceLocatorDemo : MonoBehaviour
    {
        // ── Setup: Register services ─────────────────────────────────────────

        // [SerializeField]
        // public IAdsService AdsServiceImpl;
        //
        // private void Awake()
        // {
        //     // Đăng ký Null implementations làm fallback
        //     // Trong production thay bằng implementation thật (AdMobAdsService, FirebaseTrackingService, ...)
        //     ServiceLocator.Instance.Register<IAdsService>(new NullAdsService());
        //     ServiceLocator.Instance.Register<ILocalizationService>(new NullLocalizationService());
        //     ServiceLocator.Instance.Register<ITrackingService>(new NullTrackingService());
        // }
        //
        // private void OnDestroy()
        // {
        //     ServiceLocator.Instance.Unregister<IAdsService>();
        //     ServiceLocator.Instance.Unregister<ILocalizationService>();
        //     ServiceLocator.Instance.Unregister<ITrackingService>();
        // }

        // ── Ads ──────────────────────────────────────────────────────────────

        [ContextMenu("Show Banner")]
        public void ShowBanner()
        {
            var ads = this.GetService<IAdsService>();
            ads.ShowBanner();
            Debug.Log($"[Demo] ShowBanner called via {ads.GetType().Name}");
        }

        [ContextMenu("Show Rewarded")]
        public void ShowRewarded()
        {
            var ads = this.GetService<IAdsService>();
            ads.ShowRewarded(rewarded =>
            {
                Debug.Log($"[Demo] ShowRewarded result: {rewarded}");
            });
        }

        // ── Localization ─────────────────────────────────────────────────────

        [ContextMenu("Get Localized Text")]
        public void GetLocalizedText()
        {
            var loc = this.GetService<ILocalizationService>();
            var text = loc.GetText("ui_start_button");
            Debug.Log($"[Demo] Localized text: '{text}' | Language: {loc.CurrentLanguage}");
        }

        [ContextMenu("Set Language Vietnamese")]
        public void SetLanguageVietnamese()
        {
            var loc = this.GetService<ILocalizationService>();
            loc.SetLanguage("vi");
            Debug.Log($"[Demo] Language set to: {loc.CurrentLanguage}");
        }

        // ── Tracking ─────────────────────────────────────────────────────────

        [ContextMenu("Track Event")]
        public void TrackEvent()
        {
            var tracking = this.GetService<ITrackingService>();
            tracking.TrackEvent("level_complete", new Dictionary<string, object>
            {
                { "level", 1 },
                { "score", 3500 },
                { "time_seconds", 42f }
            });
            Debug.Log("[Demo] TrackEvent 'level_complete' sent.");
        }

        // ── Extension Method style (via ServiceLocatorExtensions) ─────────────

        [ContextMenu("TryGet AdsService")]
        public void TryGetAdsService()
        {
            if (this.TryGetService<IAdsService>(out var ads))
            {
                Debug.Log($"[Demo] TryGet success: {ads.GetType().Name}");
            }
            else
            {
                Debug.LogWarning("[Demo] TryGet failed: IAdsService not registered.");
            }
        }
    }
}