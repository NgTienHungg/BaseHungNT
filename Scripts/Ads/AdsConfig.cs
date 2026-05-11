using Sirenix.OdinInspector;
using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// ScriptableObject chứa tất cả ad unit keys cho từng SDK.
    /// Đặt tại Resources/Configs/AdsConfig.asset.
    /// Tạo qua menu: Assets > Create > HungNT > Ads > Ads Config.
    /// </summary>
    [CreateAssetMenu(fileName = "AdsConfig", menuName = "HungNT/Ads/Ads Config")]
    public class AdsConfig : ScriptableObject
    {
        private const string TABS = "Tabs";

        // ╔════════════════════════════════════════════════════════════════════╗
        // ║  AdMob                                                            ║
        // ╚════════════════════════════════════════════════════════════════════╝

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/App ID"), GUIColor(0.6f, 0.9f, 0.6f)]
        [LabelText("Android")]
        [SerializeField] private string admobAppIdAndroid = "ca-app-pub-3940256099942544~3347511713";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/App ID"), GUIColor(0.6f, 0.9f, 0.6f)]
        [LabelText("iOS")]
        [SerializeField] private string admobAppIdIOS = "ca-app-pub-3940256099942544~1458002511";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/App Open"), GUIColor(0.9f, 0.85f, 0.5f)]
        [LabelText("Android")]
        [SerializeField] private string admobAppOpenIdAndroid = "ca-app-pub-3940256099942544/9257395921";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/App Open"), GUIColor(0.9f, 0.85f, 0.5f)]
        [LabelText("iOS")]
        [SerializeField] private string admobAppOpenIdIOS = "ca-app-pub-3940256099942544/5575463023";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Banner"), GUIColor(0.5f, 0.8f, 1f)]
        [LabelText("Android")]
        [SerializeField] private string admobBannerIdAndroid = "ca-app-pub-3940256099942544/9214589741";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Banner"), GUIColor(0.5f, 0.8f, 1f)]
        [LabelText("iOS")]
        [SerializeField] private string admobBannerIdIOS = "ca-app-pub-3940256099942544/2435281174";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Interstitial"), GUIColor(1f, 0.7f, 0.5f)]
        [LabelText("Android")]
        [SerializeField] private string admobInterIdAndroid = "ca-app-pub-3940256099942544/1033173712";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Interstitial"), GUIColor(1f, 0.7f, 0.5f)]
        [LabelText("iOS")]
        [SerializeField] private string admobInterIdIOS = "ca-app-pub-3940256099942544/4411468910";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Rewarded"), GUIColor(0.9f, 0.6f, 0.9f)]
        [LabelText("Android")]
        [SerializeField] private string admobRewardedIdAndroid = "ca-app-pub-3940256099942544/5224354917";

        [TabGroup(TABS, "AdMob")]
        [TitleGroup(TABS + "/AdMob/Rewarded"), GUIColor(0.9f, 0.6f, 0.9f)]
        [LabelText("iOS")]
        [SerializeField] private string admobRewardedIdIOS = "ca-app-pub-3940256099942544/1712485313";

        // ╔════════════════════════════════════════════════════════════════════╗
        // ║  MAX (AppLovin)                                                   ║
        // ╚════════════════════════════════════════════════════════════════════╝

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/SDK Key"), GUIColor(0.6f, 0.9f, 0.6f)]
        [LabelText("SDK Key")]
        [SerializeField] private string maxSdkKey = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Banner"), GUIColor(0.5f, 0.8f, 1f)]
        [LabelText("Android")]
        [SerializeField] private string maxBannerIdAndroid = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Banner"), GUIColor(0.5f, 0.8f, 1f)]
        [LabelText("iOS")]
        [SerializeField] private string maxBannerIdIOS = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Interstitial"), GUIColor(1f, 0.7f, 0.5f)]
        [LabelText("Android")]
        [SerializeField] private string maxInterIdAndroid = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Interstitial"), GUIColor(1f, 0.7f, 0.5f)]
        [LabelText("iOS")]
        [SerializeField] private string maxInterIdIOS = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Rewarded"), GUIColor(0.9f, 0.6f, 0.9f)]
        [LabelText("Android")]
        [SerializeField] private string maxRewardedIdAndroid = "";

        [TabGroup(TABS, "MAX")]
        [TitleGroup(TABS + "/MAX/Rewarded"), GUIColor(0.9f, 0.6f, 0.9f)]
        [LabelText("iOS")]
        [SerializeField] private string maxRewardedIdIOS = "";

        // ╔════════════════════════════════════════════════════════════════════╗
        // ║  IronSource                                                       ║
        // ╚════════════════════════════════════════════════════════════════════╝

        [TabGroup(TABS, "IronSource")]
        [TitleGroup(TABS + "/IronSource/App Key"), GUIColor(0.6f, 0.9f, 0.6f)]
        [LabelText("Android")]
        [SerializeField] private string ironSourceAppKeyAndroid = "";

        [TabGroup(TABS, "IronSource")]
        [TitleGroup(TABS + "/IronSource/App Key"), GUIColor(0.6f, 0.9f, 0.6f)]
        [LabelText("iOS")]
        [SerializeField] private string ironSourceAppKeyIOS = "";

        // ── Platform Helpers ─────────────────────────────────────────────────

        public string AdMobAppId =>
#if UNITY_ANDROID
            admobAppIdAndroid;
#elif UNITY_IOS
            admobAppIdIOS;
#else
            "";
#endif

        public string AdMobAppOpenId =>
#if UNITY_ANDROID
            admobAppOpenIdAndroid;
#elif UNITY_IOS
            admobAppOpenIdIOS;
#else
            "";
#endif

        public string AdMobBannerId =>
#if UNITY_ANDROID
            admobBannerIdAndroid;
#elif UNITY_IOS
            admobBannerIdIOS;
#else
            "";
#endif

        public string AdMobInterId =>
#if UNITY_ANDROID
            admobInterIdAndroid;
#elif UNITY_IOS
            admobInterIdIOS;
#else
            "";
#endif

        public string AdMobRewardedId =>
#if UNITY_ANDROID
            admobRewardedIdAndroid;
#elif UNITY_IOS
            admobRewardedIdIOS;
#else
            "";
#endif

        public string MaxSdkKey =>
#if UNITY_ANDROID || UNITY_IOS
            maxSdkKey;
#else
            "";
#endif

        public string MaxBannerId =>
#if UNITY_ANDROID
            maxBannerIdAndroid;
#elif UNITY_IOS
            maxBannerIdIOS;
#else
            "";
#endif

        public string MaxInterId =>
#if UNITY_ANDROID
            maxInterIdAndroid;
#elif UNITY_IOS
            maxInterIdIOS;
#else
            "";
#endif

        public string MaxRewardedId =>
#if UNITY_ANDROID
            maxRewardedIdAndroid;
#elif UNITY_IOS
            maxRewardedIdIOS;
#else
            "";
#endif

        public string IronSourceAppKey =>
#if UNITY_ANDROID
            ironSourceAppKeyAndroid;
#elif UNITY_IOS
            ironSourceAppKeyIOS;
#else
            "";
#endif
    }
}
