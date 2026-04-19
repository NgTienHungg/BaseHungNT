using System;
using DG.Tweening;
using UnityEngine;
using Object = UnityEngine.Object;

public static class TransformExtenstion
{
    public static Transform GetChildByName(this Transform transform, string name, bool canCreateIfNull = false)
    {
        var child = transform.Find(name);

        if (child == null && canCreateIfNull)
        {
            child = new GameObject(name).transform;
            child.SetParent(transform);
            child.localPosition = Vector3.zero;
        }

        return child;
    }

    public static void DestroyChildren(this Transform transform, string name, bool exact = true)
    {
        var allTrans = transform.GetComponentsInChildren<Transform>(true);

        for (var i = allTrans.Length - 1; i >= 1; --i)
        {
            var child = allTrans[i];

            if (exact)
            {
                if (string.Compare(child.name, name, StringComparison.Ordinal) == 0)
                {
                    Object.Destroy(allTrans[i].gameObject);
                }
            }
            else
            {
                if (child.name.Contains(name))
                {
                    Object.Destroy(allTrans[i].gameObject);
                }
            }
        }
    }

    public static Bounds CombineBounds(this Transform transform)
    {
        var renderers = transform.GetComponentsInChildren<Renderer>();
        var combinedBounds = renderers[0].bounds;
        for (var i = 1; i < renderers.Length; ++i)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        return combinedBounds;
    }

    public static Transform ChangeZ(this Transform transform, float newZ)
    {
        var pos = transform.position;
        pos.z = newZ;
        transform.position = pos;
        return transform;
    }

    public static Transform ChangeLocalZ(this Transform transform, float newZ)
    {
        var pos = transform.localPosition;
        pos.z = newZ;
        transform.localPosition = pos;
        return transform;
    }

    public static RectTransform ChangeZ(this RectTransform rectTransform, float newZ)
    {
        return (RectTransform)ChangeZ(rectTransform.transform, newZ);
    }

    public static RectTransform ChangeLocalZ(this RectTransform rectTransform, float newZ)
    {
        return (RectTransform)ChangeLocalZ(rectTransform.transform, newZ);
    }

    public static void DOShakeHorizontal(this Transform transform, float duration = 0.25f, float strength = 5f, int vibrato = 20)
    {
        if (transform == null) return;
        Vector3 shakeStrength = new Vector3(strength, 0, 0);
        transform.DOShakePosition(duration, shakeStrength, vibrato, 90, false, true);
    }

    public static void DOScaleDownImmediate(this Transform transform, float startScale = 1.15f, float targetScale = 1f, float duration = 0.2f, Ease ease = Ease.OutCubic)
    {
        transform.DOKill();
        transform.localScale = Vector3.one * startScale;
        transform.DOScale(targetScale, duration).SetEase(ease);
    }
}