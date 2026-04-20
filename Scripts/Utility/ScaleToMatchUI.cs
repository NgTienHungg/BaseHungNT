using UnityEngine;

namespace HungNT
{
    /// <summary>
    /// Tự động scale GameObject (SpriteRenderer) để khớp với kích thước của một UI (RectTransform).
    /// Dùng khi bạn muốn một object thường (non-UI) fit chính xác với vùng hiển thị của UI trên canvas.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class ScaleToMatchUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _uiTarget;
        [SerializeField] private bool _matchOnStart = true;
        [SerializeField] private bool _matchOnEnable = false;

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            if (_matchOnStart)
            {
                MatchWithUI();
            }
        }

        private void OnEnable()
        {
            if (_matchOnEnable)
            {
                MatchWithUI();
            }
        }

        /// <summary>
        /// Scale và căn giữa object để vừa khít với kích thước world space của RectTransform UI target.
        /// </summary>
        public void MatchWithUI()
        {
            if (_uiTarget == null || _spriteRenderer == null || _spriteRenderer.sprite == null)
            {
                this.LogWarning("Missing UI target or sprite.");
                return;
            }

            // Lấy kích thước UI tính theo world space
            Vector3[] corners = new Vector3[4];
            _uiTarget.GetWorldCorners(corners);
            float worldWidth = Vector3.Distance(corners[0], corners[3]); // chiều dọc (Y)
            float worldHeight = Vector3.Distance(corners[0], corners[1]); // chiều ngang (X)

            // Kích thước sprite theo đơn vị world
            Sprite sprite = _spriteRenderer.sprite;
            float spriteWidth = sprite.bounds.size.x;
            float spriteHeight = sprite.bounds.size.y;

            // Scale đều theo chiều nhỏ hơn để vừa với UI
            float scale = Mathf.Min(worldWidth / spriteWidth, worldHeight / spriteHeight);

            // Apply scale
            transform.localScale = Vector3.one * scale;

            // Căn giữa object trùng với vị trí UI
            Vector3 targetPosition = _uiTarget.position;
            targetPosition.z = transform.position.z; // giữ nguyên Z
            transform.position = targetPosition;
        }
    }
}