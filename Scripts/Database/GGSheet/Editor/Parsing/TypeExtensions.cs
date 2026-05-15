using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public static class TypeExtensions
    {
        public static bool IsAsset(this Type type)
        {
            return typeof(ScriptableObject).IsAssignableFrom(type)
                   || typeof(Texture).IsAssignableFrom(type)
                   || typeof(Sprite).IsAssignableFrom(type)
                   || typeof(AudioClip).IsAssignableFrom(type)
                   || typeof(GameObject).IsAssignableFrom(type);
        }

        public static bool IsAssetReference(this Type type) => false;

        public static bool IsNumeric(this Type myType)
        {
            HashSet<Type> numeric = new HashSet<Type>
            {
                typeof(int), typeof(double), typeof(decimal),
                typeof(long), typeof(short), typeof(sbyte),
                typeof(byte), typeof(ulong), typeof(ushort),
                typeof(uint), typeof(float)
            };

            return numeric.Contains(Nullable.GetUnderlyingType(myType) ?? myType);
        }
    }
}
