using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HungNT.Editor
{
    public static class ShortcutEditor
    {
        // %: ctrl, #: shift, &: alt

        [MenuItem("HungNT/Create/New Game Object %#g")]
        private static void NewGameObject()
        {
            GameObject go = new GameObject();
            go.transform.parent = Selection.activeTransform;
            Selection.activeTransform = go.transform;

            // check if UI
            if (go.transform.parent.TryGetComponent<RectTransform>(out var rectParent))
            {
                go.AddComponent<RectTransform>();
            }

            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            go.transform.localRotation = Quaternion.identity;
        }

        [MenuItem("HungNT/Create/New Text Mesh Pro %#t")]
        private static void NewTextMeshPro()
        {
            GameObject go = new GameObject("Text");
            go.transform.parent = Selection.activeTransform;
            Selection.activeTransform = go.transform;

            TextMeshProUGUI t = go.AddComponent<TextMeshProUGUI>();
            t.transform.localPosition = Vector3.zero;
            t.transform.localScale = Vector3.one;
            t.transform.localRotation = Quaternion.identity;
            t.SetText("New Text");
            // t.color = new Color(1, 0.98f, 0.89f, 1);
            // t.color = Color.white;
            // t.fontSize = 30;
            // t.fontSizeMin = 10;
            // t.fontSizeMax = 30;
            // t.enableAutoSizing = false;
            // t.font = FontAsset;
            // t.fontStyle = FontStyles.Normal;
            // t.raycastTarget = false;
            // t.horizontalAlignment = HorizontalAlignmentOptions.Center;
            // t.verticalAlignment = VerticalAlignmentOptions.Geometry;

            // var localize = go.AddComponent(typeof(Localize)).GetComponent<Localize>();
            // localize.Term = "-";
            // UpdateLocalize();
        }

        [MenuItem("HungNT/Create/New Image %#i")]
        private static void NewImage()
        {
            GameObject go = new GameObject("Image");
            go.transform.parent = Selection.activeTransform;
            Selection.activeTransform = go.transform;

            Image image = go.AddComponent<Image>();
            image.transform.localPosition = Vector3.zero;
            image.transform.localScale = Vector3.one;
            image.transform.localRotation = Quaternion.identity;
            image.raycastTarget = false;
            // image.sprite = SpriteDefault;
            // image.maskable = false;
        }
    }
}