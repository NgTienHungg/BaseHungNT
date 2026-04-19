using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;

namespace WingsMob
{
    public class KeyDropdownAttribute : ValueDropdownAttribute
    {
        public KeyDropdownAttribute(Type keyType) : base($"@{keyType.Name}.GetDropdown()")
        {
        }
    }

// #if UNITY_EDITOR
//     public static class KeyDropdownUtils
//     {
//         // Nhận string thay vì Type
//         public static IEnumerable GetKeys(string typeFullName)
//         {
//             WMLog.Log("typeFullName: " + typeFullName);
//
//             var valueDropdownItems = new ValueDropdownList<string>();
//
//             // Convert string thành Type
//             var type = Type.GetType(typeFullName);
//             if (type == null)
//             {
//                 WMLog.LogError("Cannot find type: " + typeFullName);
//                 return valueDropdownItems;
//             }
//
//             WMLog.Log("Type found: " + type.Name);
//
//             var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static)
//                 .Where(f => f.IsLiteral && f.FieldType == typeof(string));
//
//             foreach (var field in fields)
//             {
//                 var value = (string)field.GetValue(null);
//                 WMLog.Log($"Adding: {field.Name} = {value}");
//                 valueDropdownItems.Add(field.Name, value);
//             }
//
//             return valueDropdownItems;
//         }
//     }
// #endif
}

