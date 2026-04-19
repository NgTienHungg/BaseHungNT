using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

#if HUNGNT_TWEEN
namespace WingsMob.HungNT
{
    public class TweenAddDelay : MonoBehaviour
    {
        [SerializeField] private float _delayAdd;
        [SerializeField] private bool _applyAllChildren = true;

        [ShowIf("@!_applyAllChildren")]
        [SerializeField] private List<UITweenBase> _specificTweens;

        private void Awake()
        {
            var tweensToApply = new List<UITweenBase>();
            tweensToApply.AddRange(_applyAllChildren ? GetComponentsInChildren<UITweenBase>() : _specificTweens);

            // apply
            foreach (var tween in tweensToApply)
            {
                if (tween == null) continue;
                tween.OverrideDelay(true, tween.DelayIn + _delayAdd, tween.DelayOut + _delayAdd);
            }
        }
    }
}
#endif