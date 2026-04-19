using UnityEngine;
using UnityEngine.UI;

namespace WingsMob.HungNT
{
    [RequireComponent(typeof(Image))]
    public class ScrollTextureUI : MonoBehaviour
    {
        [SerializeField] private Vector2 _speed = new Vector2(0.1f, 0.1f);

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            _image.material = new Material(_image.material); //Clone the original material
        }

        private void FixedUpdate()
        {
            _image.material.mainTextureOffset += _speed * Time.fixedDeltaTime;
        }
    }
}