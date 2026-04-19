#if HUNGNT_TWEEN
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT
{
    [CreateAssetMenu(menuName = TweenConstant.TweenPresetMenuName, fileName = TweenConstant.TweenPresetFileName)]
    public class TweenPreset : ScriptableObject
    {
        [HorizontalGroup("In"), LabelWidth(100)]
        public Ease easeIn = Ease.Linear;

        [HorizontalGroup("Out"), LabelWidth(100)]
        public Ease easeOut = Ease.Linear;

        [HorizontalGroup("In"), LabelWidth(100)]
        public float durationIn = 0.25f;

        [HorizontalGroup("Out"), LabelWidth(100)]
        public float durationOut = 0.25f;
    }
}
#endif