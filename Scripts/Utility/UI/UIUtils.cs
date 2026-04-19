using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WingsMob.HungNT
{
    public static class UIUtils
    {
        private static PointerEventData eventDataCurrentPosition;
        private static List<RaycastResult> results;

        public static bool IsMouseOverUI()
        {
            eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /// <summary>
        /// Tính localScale target cho uiRect để khi scale xong nó có kích thước hiển thị bằng spriteRenderer.
        /// Nếu wantCover = false -> fit inside (scale = min); true -> cover (scale = max).
        /// worldCamera optional: nếu null sẽ lấy Camera.main hoặc canvas.worldCamera.
        /// </summary>
        public static float CalculateScaleToUIMatchWithSprite(RectTransform uiRect, SpriteRenderer spriteRenderer, bool isScaleUI)
        {
            if (uiRect == null || spriteRenderer == null) return 1f;

            Canvas canvas = uiRect.GetComponentInParent<Canvas>();
            if (canvas == null) return 1f;

            // Chỉ hỗ trợ ScreenSpace - Camera ở đây (theo yêu cầu)
            Camera cam = canvas.worldCamera ?? Camera.main;
            if (cam == null) return 1f;

            const float EPS = 1e-6f;

            // --- Sprite: lấy kích thước hiển thị trên màn hình (pixels) ---
            // Dùng spriteRenderer.bounds (world-space AABB) rồi chuyển sang screen points
            Bounds spBounds = spriteRenderer.bounds;
            Vector3 spMinWorld = spBounds.min;
            Vector3 spMaxWorld = spBounds.max;
            Vector3 spMinScreen = cam.WorldToScreenPoint(spMinWorld);
            Vector3 spMaxScreen = cam.WorldToScreenPoint(spMaxWorld);
            float spriteScreenW = Mathf.Abs(spMaxScreen.x - spMinScreen.x);
            float spriteScreenH = Mathf.Abs(spMaxScreen.y - spMinScreen.y);

            if (spriteScreenW < EPS || spriteScreenH < EPS) return 1f;

            // --- UI: lấy kích thước hiển thị trên màn hình (pixels) ---
            Vector3[] corners = new Vector3[4];
            uiRect.GetWorldCorners(corners); // corners ở world space (0=bl,1=br,2=tr,3=tl)
            Vector3 uiBLScreen = cam.WorldToScreenPoint(corners[0]);
            Vector3 uiTRScreen = cam.WorldToScreenPoint(corners[2]);
            float uiScreenW = Mathf.Abs(uiTRScreen.x - uiBLScreen.x);
            float uiScreenH = Mathf.Abs(uiTRScreen.y - uiBLScreen.y);

            if (uiScreenW < EPS || uiScreenH < EPS) return 1f;

            // --- Tính multiplier để UI match sprite (fit inside) ---
            float mulX = spriteScreenW / uiScreenW;
            float mulY = spriteScreenH / uiScreenH;
            float multiplier = Mathf.Min(mulX, mulY); // fit: chọn nhỏ hơn; thay bằng Mathf.Max nếu muốn cover

            // Trả về multiplier để nhân vào uiRect.localScale
            return isScaleUI ? multiplier : 1 / multiplier;
        }
    }
}