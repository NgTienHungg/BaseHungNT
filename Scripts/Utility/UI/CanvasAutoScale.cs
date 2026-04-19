#if !HUNGNT_WINGSMOB
using UnityEngine;
using UnityEngine.UI;

namespace WingsMob.HungNT
{
    /// <summary>
    /// Automatically adjusts the CanvasScaler matchWidthOrHeight based on screen ratio.
    /// </summary>
    [RequireComponent(typeof(CanvasScaler))]
    public class CanvasAutoScale : MonoBehaviour
    {
        private const float TABLET_RATIO_THRESHOLD = 1.6f;

        [SerializeField] private CanvasScaler _canvasScaler;
        [SerializeField] private bool _isAutoScale = true;
        [SerializeField] private float _scaleTablet = 1f;
        [SerializeField] private float _scaleMobile = 0f;

        private void OnValidate()
        {
            _canvasScaler = GetComponent<CanvasScaler>();
        }

        private void Start()
        {
            if (_isAutoScale)
            {
                DoScale();
            }
        }

        public void DoScale()
        {
            float ratio = (float)Screen.height / Screen.width;
            _canvasScaler.matchWidthOrHeight = ratio < TABLET_RATIO_THRESHOLD ? _scaleTablet : _scaleMobile;
        }

        private void OnRectTransformDimensionsChange()
        {
            if (_isAutoScale)
            {
                // this.Log("Screen size changed, rescale canvas");
                DoScale();
            }
        }
    }
}
#endif