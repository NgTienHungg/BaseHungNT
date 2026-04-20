using Unity.VisualScripting;
using UnityEngine;

namespace HungNT
{
    public static class ComponentExtensions
    {
        public static void SetActive<T>(this T component, bool isActive) where T : Component
        {
            component.gameObject.SetActive(isActive);
        }

        public static T GetOrAddComponent<T>(this Component component) where T : Component
        {
            return component.gameObject.GetOrAddComponent<T>();
        }

        public static void DestroyGameObject<T>(this T component) where T : Component
        {
            Object.Destroy(component.gameObject);
        }

        public static void DestroyGameObject<T>(this T component, float delay) where T : Component
        {
            Object.Destroy(component.gameObject, delay);
        }

        public static void DestroyGameObjectImmediate<T>(this T component) where T : Component
        {
            Object.DestroyImmediate(component.gameObject);
        }
    }
}