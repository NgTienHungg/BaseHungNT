using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.Demo
{
    public class ColorKey : BaseKeyDropdown<ColorKey>
    {
        public const string Red = "red";
        public const string Blue = "blue";
        public const string Green = "green";
        public const string Yellow = "yellow";
    }

    public class KeyDropdownDemo : MonoBehaviour
    {
        // [KeyDropdown(typeof(ColorKey))]
        // [ValueDropdown("@ColorKey.GetDropdown()")]
        // [ValueDropdown(nameof(ColorKey.GetDropdown))]
        public string colorKey;

        [Button]
        private void LogColorKey()
        {
            Debug.Log($"colorKey: {colorKey}");
        }
    }
}