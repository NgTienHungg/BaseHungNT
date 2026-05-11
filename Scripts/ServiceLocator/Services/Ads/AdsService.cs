using System;
using UnityEngine;

namespace HungNT
{
    public class AdsService : MonoBehaviour, IAdsService
    {
        public void ShowBanner()
        {
            throw new NotImplementedException();
        }

        public void HideBanner()
        {
            throw new NotImplementedException();
        }

        public bool IsInterstitialReady()
        {
            throw new NotImplementedException();
        }

        public void ShowInterstitial(Action onComplete)
        {
            throw new NotImplementedException();
        }

        public bool IsRewardedReady()
        {
            throw new NotImplementedException();
        }

        public void ShowRewarded(Action<bool> onComplete)
        {
            throw new NotImplementedException();
        }
    }
}