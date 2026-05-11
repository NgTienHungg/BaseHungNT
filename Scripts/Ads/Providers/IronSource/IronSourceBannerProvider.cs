#if USE_IRONSOURCE
namespace HungNT
{
    public class IronSourceBannerProvider : IBannerAdProvider
    {
        private bool _isBannerShowing;

        public void Initialize(AdsConfig config)
        {
            var appKey = config.IronSourceAppKey;
            if (string.IsNullOrEmpty(appKey)) return;

            IronSource.Agent.init(appKey, IronSourceAdUnits.BANNER);
            this.Log("Banner initialized.");
        }

        public void Dispose()
        {
            IronSource.Agent.destroyBanner();
        }

        public void ShowBanner()
        {
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, IronSourceBannerPosition.BOTTOM);
            _isBannerShowing = true;
        }

        public void HideBanner()
        {
            IronSource.Agent.hideBanner();
            _isBannerShowing = false;
        }
    }
}
#endif
