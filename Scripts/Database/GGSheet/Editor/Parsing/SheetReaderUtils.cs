using System;
using System.ComponentModel;
using System.Reflection;

namespace HungNT.Database.Editor
{
    public static class SheetReaderUtils
    {
        public static Type GetElementTypeFromFieldInfo(FieldInfo tmp)
        {
            string fullName = string.Empty;
            if (tmp.FieldType.IsArray)
            {
                if (tmp.FieldType.FullName != null)
                    fullName = tmp.FieldType.FullName.Substring(0, tmp.FieldType.FullName.Length - 2);
            }
            else
            {
                fullName = tmp.FieldType.FullName;
            }

            return GetType(fullName);
        }

        public static Type GetType(string strFullyQualifiedName)
        {
            Type type = Type.GetType(strFullyQualifiedName);
            if (type != null)
                return type;

            type = Type.GetType(strFullyQualifiedName + ", Assembly-CSharp");
            if (type != null)
                return type;

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = asm.GetType(strFullyQualifiedName);
                if (type != null)
                    return type;
            }

            throw new Exception("Type is null: " + strFullyQualifiedName);
        }

        public static bool IsValidKeyFormat(string key) => key.Equals(key.ToLowerInvariant(), StringComparison.Ordinal);

        public static bool IsPrimitive(FieldInfo tmp, bool isCustomPrimitiveArray)
        {
            Type type = tmp.FieldType.IsArray && isCustomPrimitiveArray
                ? GetElementTypeFromFieldInfo(tmp)
                : tmp.FieldType;

            return IsPrimitive(type);
        }

        public static bool IsPrimitive(Type type) =>
            StringConverter.IsSimpleConvertable(type) || type.IsEnum;

        public static string ConvertSnakeCaseToCamelCase(string snakeCase)
        {
            var strings = snakeCase.Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length == 0)
                return snakeCase;

            var result = strings[0];
            for (int i = 1; i < strings.Length; i++)
            {
                var currentString = strings[i];
                if (currentString.Length > 0)
                    result += char.ToUpperInvariant(currentString[0]) + currentString.Substring(1);
            }

            return result;
        }

        public static object GetDefaultValue(Type enumType)
        {
            var attribute = enumType.GetCustomAttribute<DefaultValueAttribute>(inherit: false);
            if (attribute != null)
                return attribute.Value;

            var innerType = enumType.GetEnumUnderlyingType();
            var zero = Activator.CreateInstance(innerType);
            if (enumType.IsEnumDefined(zero))
                return zero;

            var values = enumType.GetEnumValues();
            return values.GetValue(0);
        }
    }
}
