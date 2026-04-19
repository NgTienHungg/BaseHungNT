#if HUNGNT_TWEEN
using System.Linq;
using UnityEngine;

namespace WingsMob.HungNT
{
    /// <summary>
    /// Control delay by index in hierarchy
    /// </summary>
    public class TweenDelayByIndexControl : MonoBehaviour
    {
        [SerializeField] private float delayInterval = 0.05f;
        [SerializeField] private float startDelay = 0f;

        private void Awake()
        {
            var delay = startDelay;
            transform.GetComponentsInChildren<TweenDelayByIndex>().ToList().ForEach(tween =>
            {
                tween.OverrideDelay(true, delay, 0f);
                delay += delayInterval;
            });
        }
    }
}
#endif