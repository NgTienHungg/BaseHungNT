using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    [Serializable]
    public class TweenConfig
    {
        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Left")]
        public float Duration = 0.25f;

        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Left")]
        public float Delay = 0f;

        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Left")]
        public int Loops = 1;

        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Right")]
        [HideIf("UseCurve")]
        public Ease Ease = Ease.Linear;

        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Right")]
        [ShowIf("UseCurve")]
        public AnimationCurve Curve = AnimationCurve.Linear(0, 0, 1, 1);

        [HorizontalGroup("Horizon")] [VerticalGroup("Horizon/Right")]
        public bool UseCurve = false;
    }

    public static class TweenConfigExtensions
    {
        public static Tween ApplyTweenConfig(this Tween tween, TweenConfig config)
        {
            return tween.ApplyEase(config)
                .SetDelay(config.Delay)
                .SetLoops(config.Loops);
        }

        public static Tween ApplyEase(this Tween tween, TweenConfig config)
        {
            return config.UseCurve ? tween.SetEase(config.Curve) : tween.SetEase(config.Ease);
        }
    }
}