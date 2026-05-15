using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public static class StringConverter
    {
        private delegate object Converter(string value);

        private static readonly Dictionary<Type, Converter> SimpleConverters = new Dictionary<Type, Converter>
        {
            { typeof(string), ToString },
            { typeof(int), ToInt32 },
            { typeof(short), ToInt16 },
            { typeof(long), ToInt64 },
            { typeof(uint), ToUInt32 },
            { typeof(ushort), ToUInt16 },
            { typeof(ulong), ToUInt64 },
            { typeof(byte), ToByte },
            { typeof(sbyte), ToSByte },
            { typeof(char), ToChar },
            { typeof(bool), ToBool },
            { typeof(float), ToSingle },
            { typeof(double), ToDouble },
            { typeof(decimal), ToDecimal },
            { typeof(DateTime), ToDateTime },
            { typeof(Vector2), ToVector2 },
            { typeof(Vector3), ToVector3 },
            { typeof(Vector4), ToVector4 },
            { typeof(Vector2Int), ToVector2Int },
            { typeof(Vector3Int), ToVector3Int },
            { typeof(Bounds), ToBounds },
            { typeof(BoundsInt), ToBoundsInt },
            { typeof(Rect), ToRect },
            { typeof(RectInt), ToRectInt },
            { typeof(Color), ToColor },
            { typeof(Color32), ToColor32 },
            { typeof(Sprite), ToSprite },
        };

        public static T Convert<T>(string value) => (T)Convert(typeof(T), value);

        public static T[] Convert<T>(string value, string delimiter) =>
            Convert(typeof(T), value, delimiter) as T[];

        public static object Convert(Type type, string value, string delimiter)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            {
                Type elementType = type.GetGenericArguments()[0];
                string[] split = value.Split(new[] { delimiter }, StringSplitOptions.None);
                IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                foreach (var item in split)
                {
                    object convertedItem = Convert(elementType, item.Trim());
                    list.Add(convertedItem);
                }

                return list;
            }

            if (type.IsArray)
            {
                Type arrayElementType = type.GetElementType();
                string[] arraySplit = value.Split(new[] { delimiter }, StringSplitOptions.None);
                Array result = Array.CreateInstance(arrayElementType, arraySplit.Length);
                for (int ix = 0; ix < arraySplit.Length; ix++)
                    result.SetValue(Convert(arrayElementType, arraySplit[ix].Trim()), ix);

                return result;
            }

            return Convert(type, value);
        }

        public static object Convert(Type type, string value)
        {
            if (SimpleConverters.TryGetValue(type, out Converter converter))
                return converter(value);

            if (type.IsEnum)
            {
                object obj;
                if (long.TryParse(value, out long result))
                {
                    try
                    {
                        obj = Enum.ToObject(type, result);
                    }
                    catch
                    {
                        obj = System.Convert.ChangeType(0, type);
                    }
                }
                else if (!Enum.TryParse(type, value, true, out obj))
                {
                    obj = System.Convert.ChangeType(0, type);
                }

                return obj;
            }

            if (type.IsAsset())
            {
                string[] guids = AssetDatabase.FindAssets($"{value} t:{type.FullName}");
                if (guids != null && guids.Length > 0)
                    return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), type);

                return null;
            }

            ConstructorInfo constructor = type.GetConstructor(new[] { typeof(string) });
            if (constructor != null)
                return constructor.Invoke(new object[] { value });

            throw new NotSupportedException($"StringConverter cannot convert to {type.FullName}");
        }

        public static bool IsSimpleConvertable(Type type) => SimpleConverters.ContainsKey(type);

        public static bool IsConvertable(Type type)
        {
            if (type.IsSZArray)
                return IsConvertable(type.GetElementType());

            if (SimpleConverters.ContainsKey(type))
                return true;

            if (type.IsEnum)
                return true;

            if (type.IsAsset())
                return true;

            return type.GetConstructor(new[] { typeof(string) }) != null;
        }

        private static float[] SplitCommaSeparatedFloats(string value, int minSize)
        {
            if (string.IsNullOrEmpty(value))
                return new float[minSize];

            string[] split = value.Split(',');
            float[] floats = new float[Math.Max(split.Length, minSize)];
            for (int ix = 0; ix < floats.Length; ix++)
            {
                if (ix < split.Length && float.TryParse(split[ix].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                    floats[ix] = result;
                else
                    floats[ix] = default;
            }

            return floats;
        }

        private static byte FloatToByte(float value)
        {
            if (value <= 0f)
                return 0;
            if (value >= 255f)
                return 255;
            if (value > 0f && value <= 1f)
                return (byte)(value * 255);

            return (byte)value;
        }

        private static object ToString(string value) => value;

        private static object ToInt32(string value) =>
            int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int r) ? r : default;

        private static object ToInt16(string value) =>
            short.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out short r) ? r : default;

        private static object ToInt64(string value) =>
            long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out long r) ? r : default;

        private static object ToUInt32(string value) =>
            uint.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out uint r) ? r : default;

        private static object ToUInt16(string value) =>
            ushort.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out ushort r) ? r : default;

        private static object ToUInt64(string value) =>
            ulong.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out ulong r) ? r : default;

        private static object ToByte(string value) =>
            byte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out byte r) ? r : default;

        private static object ToSByte(string value) =>
            sbyte.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out sbyte r) ? r : default;

        private static object ToChar(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(char);
            return value.Length >= 1 ? value[0] : default(char);
        }

        private static object ToBool(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(bool);

            string lower = value.ToLowerInvariant();
            if (lower == "true")
                return true;
            if (lower == "false")
                return false;
            if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                return result > 0f;

            return default(bool);
        }

        private static object ToSingle(string value) =>
            float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float r) ? r : default;

        private static object ToDouble(string value) =>
            double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out double r) ? r : default;

        private static object ToDecimal(string value) =>
            decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal r) ? r : default;

        private static object ToDateTime(string value) =>
            DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime r) ? r : default;

        private static object ToVector2(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 2);
            return new Vector2(floats[0], floats[1]);
        }

        private static object ToVector3(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 3);
            return new Vector3(floats[0], floats[1], floats[2]);
        }

        private static object ToVector4(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 4);
            return new Vector4(floats[0], floats[1], floats[2], floats[3]);
        }

        private static object ToVector2Int(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 2);
            return new Vector2Int((int)floats[0], (int)floats[1]);
        }

        private static object ToVector3Int(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 3);
            return new Vector3Int((int)floats[0], (int)floats[1], (int)floats[2]);
        }

        private static object ToRect(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 4);
            return new Rect(floats[0], floats[1], floats[2], floats[3]);
        }

        private static object ToRectInt(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 4);
            return new RectInt((int)floats[0], (int)floats[1], (int)floats[2], (int)floats[3]);
        }

        private static object ToBounds(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 6);
            return new Bounds(new Vector3(floats[0], floats[1], floats[2]), new Vector3(floats[3], floats[4], floats[5]));
        }

        private static object ToBoundsInt(string value)
        {
            float[] floats = SplitCommaSeparatedFloats(value, 6);
            return new BoundsInt((int)floats[0], (int)floats[1], (int)floats[2], (int)floats[3], (int)floats[4], (int)floats[5]);
        }

        private static object ToColor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(Color);

            if (value.Length > 0 && value[0] == '#')
            {
                if (ColorUtility.TryParseHtmlString(value, out Color result))
                    return result;
                return default(Color);
            }

            float[] floats = SplitCommaSeparatedFloats(value, 3);
            return new Color(floats[0], floats[1], floats[2], floats.Length >= 4 ? floats[3] : 1f);
        }

        private static object ToColor32(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(Color32);

            if (value.Length > 0 && value[0] == '#')
            {
                if (ColorUtility.TryParseHtmlString(value, out Color result))
                    return (Color32)result;
                return default(Color32);
            }

            float[] floats = SplitCommaSeparatedFloats(value, 3);
            return new Color32(
                FloatToByte(floats[0]),
                FloatToByte(floats[1]),
                FloatToByte(floats[2]),
                floats.Length >= 4 ? FloatToByte(floats[3]) : (byte)255
            );
        }

        private static object ToSprite(string value)
        {
            string[] guids = AssetDatabase.FindAssets($"{value} t:{nameof(Sprite)}");
            if (guids == null || guids.Length <= 0)
                return null;

            foreach (var guid in guids)
            {
                var data = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Sprite));
                if (data != null && data.name == value)
                    return data;
            }

            return AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), typeof(Sprite));
        }
    }
}
