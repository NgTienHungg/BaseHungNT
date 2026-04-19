using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class FadeExtensions
{
    /// <summary>
    /// Thay đổi alpha của 1 color.
    /// </summary>
    public static Color Fade(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }

    #region === OBJECT ===
    /// <summary>
    /// Thay đổi alpha của 1 SpriteRenderer.
    /// </summary>
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        renderer.color = renderer.color.Fade(alpha);
    }

    /// <summary>
    /// Thay đổi alpha của tất cả SpriteRenderer trong GameObject (Bao gồm cả child của nó).
    /// </summary>
    public static void FadeAll(this GameObject go, float alpha)
    {
        var spriteRenderer = go.GetComponent<SpriteRenderer>();

        if (spriteRenderer)
        {
            spriteRenderer.color = spriteRenderer.color.Fade(alpha);
        }

        foreach (Transform child in go.transform)
        {
            FadeAll(child.gameObject, alpha);
        }
    }

    /// <summary>
    /// Thay đổi alpha của tất cả SpriteRenderer trong GameObject (Bao gồm cả child của nó).
    /// </summary>
    public static void DOFadeAll(this GameObject go, float alpha, float duration = 0f, float delay = 0f, Ease ease = Ease.Linear)
    {
        var spriteRenderer = go.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.DOKill();
            spriteRenderer.DOFade(alpha, duration)
                .SetDelay(delay)
                .SetEase(ease);
        }

        foreach (Transform child in go.transform)
            DOFadeAll(child.gameObject, alpha, duration, delay, ease);
    }
    #endregion

    #region === UI ===
    public static void Fade(this CanvasGroup canvasGroup, float alpha)
    {
        canvasGroup.alpha = alpha;
    }

    public static void Fade(this Image image, float alpha)
    {
        image.color = image.color.Fade(alpha);
    }

    public static void Fade(this TMP_Text text, float alpha)
    {
        text.color = text.color.Fade(alpha);
    }

    public static void Fade(this TextMeshPro text, float alpha)
    {
        text.color = text.color.Fade(alpha);
    }

    public static void Fade(this TextMeshProUGUI text, float alpha)
    {
        text.color = text.color.Fade(alpha);
    }
    #endregion
}