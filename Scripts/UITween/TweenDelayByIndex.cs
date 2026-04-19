#if HUNGNT_TWEEN
using System.Linq;
using UnityEngine;

namespace WingsMob.HungNT
{
    /// <summary>
    /// Element has this script will be override delay for all children tweens by TweenDelayByIndexControl in parent
    /// </summary>
    public class TweenDelayByIndex : MonoBehaviour
    {
        public void OverrideDelay(bool delay, float delayIn, float delayOut)
        {
            var tweens = transform.GetComponentsInChildren<UITweenBase>().ToList();
            tweens.ForEach(tween => tween.OverrideDelay(delay, delayIn, delayOut));
        }
    }
}
#endif