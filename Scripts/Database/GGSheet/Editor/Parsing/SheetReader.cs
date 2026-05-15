using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Assertions;

namespace HungNT.Database.Editor
{
    public static class SheetReader
    {
        public const BindingFlags AUTO_BINDING_FLAGS =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField | BindingFlags.SetProperty;

        public static object Deserialize(Type type, List<Row> rows, IImportUtility util, FieldInfo fieldInfo, bool isArray = true) =>
            isArray ? CreateArray(type, rows, util, fieldInfo, true) : CreateSingle(type, rows, util, fieldInfo);

        public static object DeserializeContent(object target, Type type, FieldInfo info, List<Row> rows, IImportUtility util, bool isArray = false,
            string format = "{0}", DataSheet dataSheet = null)
        {
            object result = null;
            var table = CreateTable(rows, dataSheet);

            (bool primitiveArray, _) = GetPrimitiveArraySeparator(info);
            bool isPrimitive = SheetReaderUtils.IsPrimitive(info, primitiveArray);

            if (primitiveArray && isPrimitive)
            {
                var cols = rows[0].Values.ToArray();
                string columnName = GetFieldColumnName(info, format);
                if (table.TryGetValue(columnName, out var idx))
                    SetValuePrimitive(target, info, idx < cols.Length ? cols[idx] : string.Empty);
                else
                {
                    SetValuePrimitive(target, info, string.Empty);
                    Debug.LogWarning("Key is not exist: " + columnName);
                }

                result = info.GetValue(target);
            }
            else if (!primitiveArray && isPrimitive)
            {
                string columnName = GetFieldColumnName(info, format);
                var cols = rows[0].Values.ToArray();
                if (table.TryGetValue(columnName, out var idx))
                    SetValuePrimitive(target, info, idx < cols.Length ? cols[idx] : string.Empty);
                else
                    Debug.LogError("Key is not exist: " + columnName);

                result = info.GetValue(target);
            }
            else if (!isArray && !isPrimitive)
            {
                result = CreateSingle(type, rows, util, fieldInfo: info);
            }
            else if (isArray && !isPrimitive)
            {
                result = CreateArray(type, rows, util, info, false);
            }

            return result;
        }

        private static Dictionary<string, int> CreateTable(List<Row> rows, DataSheet dataSheet = null)
        {
            var table = new Dictionary<string, int>();
            var header = rows[0].Keys.ToArray();

            for (int i = 0; i < header.Length; i++)
            {
                string id = header[i];
                if (SheetReaderUtils.IsValidKeyFormat(id))
                {
                    var camelId = SheetReaderUtils.ConvertSnakeCaseToCamelCase(id);
                    if (!table.ContainsKey(camelId))
                        table.Add(camelId, i);
                    else
                        throw new Exception("Key is duplicate: " + id);
                }
                else
                {
                    throw new Exception("Key is not valid: " + id);
                }
            }

            return table;
        }

        private static object CreateSingle(Type type, List<Row> rows, IImportUtility util, FieldInfo fieldInfo)
        {
            var table = CreateTable(rows, null);

            if (type.IsAsset())
                return CreateUnityObject(0, 0, rows, util, table, type, fieldInfo);
            if (type.IsAssetReference())
                return CreateAddressableReference(0, 0, rows, util, table, type, fieldInfo);

            return Create(0, 0, rows, util, table, type);
        }

        private static object CreateArray(Type type, List<Row> rows, IImportUtility util, FieldInfo fieldInfo, bool hasWrapper)
        {
            var table = CreateTable(rows);
            string fieldSetValue = fieldInfo.Name;

            List<int> startRows = hasWrapper ? GetWrapperObjectIndices(0, rows) : CountNumberElement(0, 0, 0, rows);

            Array arrayValue = Array.CreateInstance(type, startRows.Count);

            var isPrimitive = SheetReaderUtils.IsPrimitive(type);
            if (isPrimitive)
            {
                if (table.TryGetValue(fieldSetValue, out var idx))
                {
                    for (int i = 0; i < arrayValue.Length; i++)
                    {
                        object rowData = GetPrimitiveValue(type, rows[startRows[i]].Values.ElementAt(idx));
                        arrayValue.SetValue(rowData, i);
                    }
                }
                else
                {
                    throw new Exception($"Not found field to set value: {fieldSetValue}");
                }
            }
            else
            {
                for (int i = 0; i < arrayValue.Length; i++)
                {
                    object rowData;
                    if (type.IsAsset())
                        rowData = CreateUnityObject(startRows[i], 0, rows, util, table, type, fieldInfo);
                    else if (type.IsAssetReference())
                        rowData = CreateAddressableReference(startRows[i], 0, rows, util, table, type, fieldInfo);
                    else
                        rowData = Create(startRows[i], 0, rows, util, table, type);

                    arrayValue.SetValue(rowData, i);
                }
            }

            return arrayValue;
        }

        private static UnityEngine.Object FindAssetByName(Type type, string name)
        {
            string[] guids = UnityEditor.AssetDatabase.FindAssets($"{name} t:{type.Name}");
            if (guids == null || guids.Length <= 0)
                return null;

            return UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]), type);
        }

        private static object CreateUnityObject(int index, int parentIndex, List<Row> rows, IImportUtility util, Dictionary<string, int> table,
            Type type, FieldInfo wrapperField, string format = "{0}")
        {
            if (!type.IsAsset())
                Debug.LogWarning("Type must be a Unity object");

            if (!type.IsArray)
            {
                var columnName = GetFieldColumnName(wrapperField, format);
                if (!table.TryGetValue(columnName, out var idx))
                    Debug.LogWarning($"Key is not exist: {columnName}");

                UnityEngine.Object singleObject = FindAssetByName(type, rows[index].Values.ElementAt(idx));
                if (typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type))
                {
                    SetValues(util, type, singleObject);
                    UnityEditor.EditorUtility.SetDirty(singleObject);
                }

                return singleObject;
            }

            var wrapperColumnName = GetFieldColumnName(wrapperField, format);
            if (!table.TryGetValue(wrapperColumnName, out var wrapperIndex))
                Debug.LogWarning($"Key is not exist: {wrapperColumnName}");

            string objectName = rows[index].Values.ElementAt(wrapperIndex);
            object variable = FindAssetByName(type, objectName);
            if (variable == null)
                throw new Exception($"Unity object of type '{type.Name}' and name '{objectName}' not found");

            FieldInfo[] fieldInfo = type.GetFields(AUTO_BINDING_FLAGS);
            foreach (FieldInfo info in fieldInfo)
            {
                var ignoredAttributes = info.GetCustomAttributes(typeof(IgnoreColumnAttribute), true);
                if (ignoredAttributes.Length > 0) continue;

                if (info.FieldType.IsAsset())
                    return CreateUnityObject(index, parentIndex, rows, util, table, info.FieldType, info);
                if (info.FieldType.IsAssetReference())
                    return CreateAddressableReference(index, parentIndex, rows, util, table, info.FieldType, info);

                return Create(index, parentIndex, rows, util, table, type, format);
            }

            return variable;
        }

        private static object CreateAddressableReference(int index, int parentIndex, List<Row> rows, IImportUtility util,
            Dictionary<string, int> table, Type type, FieldInfo wrapperField, string format = "{0}") =>
            throw new NotSupportedException(
                "GGSheet: AssetReference / Addressables is not enabled. Use direct asset types (Sprite, etc.) or paths.");

        private static object Create(int index, int parentIndex, List<Row> rows, IImportUtility util, Dictionary<string, int> table, Type type,
            string format = "{0}")
        {
            object variable = null;
            if (!type.IsAsset())
                variable = Activator.CreateInstance(type);

            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var cols = rows[index].Values.ToArray();
            foreach (FieldInfo info in fieldInfo)
            {
                var ignoredAttributes = info.GetCustomAttributes(typeof(IgnoreColumnAttribute), true);
                if (ignoredAttributes.Length > 0) continue;

                (bool primitiveArray, _) = GetPrimitiveArraySeparator(info);
                bool isPrimitive = SheetReaderUtils.IsPrimitive(info, primitiveArray);
                if (isPrimitive)
                {
                    string columnName = GetFieldColumnName(info, format);
                    if (table.TryGetValue(columnName, out var idx))
                        SetValuePrimitive(variable, info, idx < cols.Length ? cols[idx] : string.Empty);
                    else
                        Debug.LogWarning("Key is not exist: " + columnName);
                }
                else
                {
                    var fieldFormat = GetColumnFormat(info);
                    if (info.FieldType.IsArray)
                    {
                        var elementType = SheetReaderUtils.GetElementTypeFromFieldInfo(info);
                        string columnName = GetFieldColumnName(info, format);
                        int objectIndex;
                        var isElementPrimitive = SheetReaderUtils.IsPrimitive(elementType);
                        if (isElementPrimitive)
                        {
                            if (table.TryGetValue(columnName, out var value))
                            {
                                objectIndex = value;
                                Assert.IsTrue(objectIndex < cols.Length);
                            }
                            else
                            {
                                Debug.LogWarning($"Key is not exist: {columnName}");
                                continue;
                            }
                        }
                        else
                        {
                            objectIndex = GetObjectIndex(elementType, table, fieldFormat);
                        }

                        var startRows = CountNumberElement(index, objectIndex, parentIndex, rows);
                        Array arrayValue = Array.CreateInstance(elementType, startRows.Count);
                        for (int i = 0; i < arrayValue.Length; i++)
                        {
                            if (isElementPrimitive)
                            {
                                var value = rows[startRows[i]].Values.ElementAt(objectIndex);
                                arrayValue.SetValue(GetPrimitiveValue(elementType, value), i);
                            }
                            else
                            {
                                arrayValue.SetValue(Create(startRows[i], objectIndex, rows, util, table, elementType, fieldFormat), i);
                            }
                        }

                        info.SetValue(variable, arrayValue);
                    }
                    else
                    {
                        var typeName = info.FieldType.FullName;
                        if (typeName == null)
                            throw new Exception("Full name is nil");

                        Type elementType = SheetReaderUtils.GetType(typeName);
                        var objectIndex = GetObjectIndex(elementType, table);
                        var value = Create(index, objectIndex, rows, util, table, elementType, fieldFormat);
                        info.SetValue(variable, value);
                    }
                }
            }

            return variable;
        }

        private static void SetValuePrimitive(object variable, FieldInfo fieldInfo, string value)
        {
            var type = fieldInfo.FieldType;
            if (string.IsNullOrEmpty(value))
                value = GetDefaultValue(fieldInfo);

            if (type.IsArray)
            {
                (_, string customSeparator) = GetPrimitiveArraySeparator(fieldInfo);
                fieldInfo.SetValue(variable, StringConverter.Convert(type, value, customSeparator));
            }
            else
            {
                fieldInfo.SetValue(variable, GetPrimitiveValue(type, value));
            }
        }

        private static object GetPrimitiveValue(Type type, string value)
        {
            var converted = StringConverter.Convert(type, value);
            if (converted == null)
                Debug.LogWarning($"Failed to convert value '{value}' to type '{type.Name}'");

            return converted;
        }

        private static int GetObjectIndex(Type type, Dictionary<string, int> table, string format = "{0}")
        {
            int minIndex = int.MaxValue;
            FieldInfo[] fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo tmp in fieldInfo)
            {
                var fieldName = GetFieldColumnName(tmp, format);
                if (table.TryGetValue(fieldName, out var idx) && idx < minIndex)
                    minIndex = idx;
            }

            return minIndex;
        }

        private static List<int> CountNumberElement(int rowIndex, int objectIndex, int parentIndex, List<Row> rows)
        {
            var startRows = new List<int>();
            for (int i = rowIndex; i < rows.Count; i++)
            {
                var row = rows[i];
                try
                {
                    if (row.Count > objectIndex || (row.Count > objectIndex && !row.Values.ElementAt(objectIndex).Equals("#")))
                    {
                        if (objectIndex == parentIndex)
                            startRows.Add(i);
                        else if (row.Values.Count() > parentIndex && string.IsNullOrEmpty(row.Values.ElementAt(parentIndex)) || i == rowIndex)
                            startRows.Add(i);
                        else
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return startRows;
        }

        private static List<int> GetWrapperObjectIndices(int rowIndex, List<Row> rows)
        {
            var startRows = new List<int>();
            for (int i = rowIndex; i < rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(rows[i].PrimaryValue))
                    startRows.Add(i);
            }

            return startRows;
        }

        private static string GetFieldColumnName(FieldInfo fieldInfo, string format)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(ColumnNameAttribute), true);
            if (attributes.Length > 0)
            {
                var columnNameAttribute = (ColumnNameAttribute)attributes[0];
                return SheetReaderUtils.ConvertSnakeCaseToCamelCase(string.Format(format, columnNameAttribute.ColumnName));
            }

            return SheetReaderUtils.ConvertSnakeCaseToCamelCase(string.Format(format, fieldInfo.Name));
        }

        private static string GetColumnFormat(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(ColumnNameFormatAttribute), true);
            if (attributes.Length > 0)
            {
                var columnNameFormatAttribute = (ColumnNameFormatAttribute)attributes[0];
                return columnNameFormatAttribute.ColumnFormat;
            }

            return "{0}";
        }

        private static (bool, string) GetPrimitiveArraySeparator(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(PrimitiveArrayContent), true);
            if (attributes.Length > 0)
            {
                var primitiveInlinedArrayContent = (PrimitiveArrayContent)attributes[0];
                return (true, primitiveInlinedArrayContent.CustomSeparator);
            }

            return (false, "*");
        }

        private static string GetDefaultValue(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(DefaultValueAttribute), true);
            if (attributes.Length > 0)
                return ((DefaultValueAttribute)attributes[0]).Value.ToString();

            var type = fieldInfo.FieldType;
            if (type == typeof(string))
                return string.Empty;
            if (type.IsNumeric())
                return "0";
            if (type == typeof(bool))
                return "FALSE";
            if (type.IsEnum)
                return SheetReaderUtils.GetDefaultValue(type).ToString();

            Debug.LogWarning($"{type.FullName} {fieldInfo.Name} is not support default value.");
            return string.Empty;
        }

        private static ContentAttribute GetEffectiveContentAttribute(FieldInfo f) =>
            (ContentAttribute)f.GetCustomAttribute<ColumnNameAttribute>() ?? f.GetCustomAttribute<ContentAttribute>();

        public static void SetValues(IImportUtility util, Type assetType, UnityEngine.Object assetInstance)
        {
            var fieldWithAttributes1 = assetType.GetFields(AUTO_BINDING_FLAGS)
                .Where(f => Attribute.IsDefined(f, typeof(ArrayContentAttribute))).ToList();

            var arrayContentAttributeField = fieldWithAttributes1
                .FindAll(f => f.GetCustomAttribute<ArrayContentAttribute>() != null);

            // foreach (var att in arrayContentAttributeField)
            //     Debug.Log("att: " + att.GetCustomAttribute<ArrayContentAttribute>().WorksheetName);

            for (var i = 0; i < util.DataSheets.Count; i++)
            {
                var match = arrayContentAttributeField.FindIndex(t =>
                {
                    var att = t.GetCustomAttribute<ArrayContentAttribute>();
                    if (att == null) return false;
                    return att.WorksheetName == util.DataSheets[i].Id.WorksheetName
                           && att.From == util.DataSheets[i].FromCol
                           && att.To == util.DataSheets[i].ToCol;
                });

                if (match != -1)
                {
                    // DebugEx.Log("Importing: " + util.DataSheets[i].Id.WorksheetName + " - index: " + match);
                    var rows = util.DataSheets[i].GetRows(util.PrimaryKey).ToList();
                    var dataType = SheetReaderUtils.GetElementTypeFromFieldInfo(arrayContentAttributeField[match]);
                    var arrayObject = Deserialize(dataType, rows, util, arrayContentAttributeField[match], true);
                    arrayContentAttributeField[match].SetValue(assetInstance, arrayObject as Array);
                }
            }

            var fieldWithAttributes2 = assetType.GetFields(AUTO_BINDING_FLAGS)
                .FirstOrDefault(f =>
                    Attribute.IsDefined(f, typeof(ArrayContentAttribute)) &&
                    string.IsNullOrEmpty(f.GetCustomAttribute<ArrayContentAttribute>().WorksheetName));

            if (fieldWithAttributes2 != null && util.DataSheets.Count > 0)
            {
                var rows = util.DataSheets[0].GetRows(DataSheet.DEFAULT_PRIMARY_KEY).ToList();
                Debug.Log("Importing: " + util.DataSheets[0].Id.WorksheetName);
                var dataType = SheetReaderUtils.GetElementTypeFromFieldInfo(fieldWithAttributes2);
                var arrayObject = Deserialize(dataType, rows, util, fieldWithAttributes2, true);
                fieldWithAttributes2.SetValue(assetInstance, arrayObject as Array);
            }

            var fieldWithContentAttributes = assetType.GetFields(AUTO_BINDING_FLAGS)
                .Where(f =>
                    Attribute.IsDefined(f, typeof(ContentAttribute)) || Attribute.IsDefined(f, typeof(ColumnAttribute))).ToList();

            if (util.DataSheets.Count == 0)
                return;

            var defaultRows = util.DataSheets[0].GetRows(DataSheet.DEFAULT_PRIMARY_KEY).ToList();

            for (int i = 0; i < util.DataSheets.Count; i++)
            {
                var singleWorksheetRows = util.DataSheets[i].GetRows(DataSheet.DEFAULT_PRIMARY_KEY).ToList();
                foreach (var fieldWithContentAttribute in fieldWithContentAttributes)
                {
                    var contentAtt = GetEffectiveContentAttribute(fieldWithContentAttribute);
                    if (contentAtt == null)
                        continue;

                    if (contentAtt.WorksheetName == util.DataSheets[i].Id.WorksheetName)
                    {
                        var contentDataType = fieldWithContentAttribute.FieldType;
                        bool isArray = false;
                        if (contentDataType.IsArray)
                        {
                            contentDataType = contentDataType.GetElementType();
                            isArray = true;
                        }

                        var contentObject = DeserializeContent(assetInstance, contentDataType, fieldWithContentAttribute, singleWorksheetRows, util, isArray,
                            dataSheet: util.DataSheets[i]);
                        fieldWithContentAttribute.SetValue(assetInstance, contentObject);
                        continue;
                    }

                    if (string.IsNullOrEmpty(contentAtt.WorksheetName))
                    {
                        var contentDataType = fieldWithContentAttribute.FieldType;
                        bool isArray = false;
                        if (contentDataType.IsArray)
                        {
                            contentDataType = contentDataType.GetElementType();
                            isArray = true;
                        }

                        var contentObject = DeserializeContent(assetInstance, contentDataType, fieldWithContentAttribute, defaultRows, util, isArray,
                            dataSheet: util.DataSheets[i]);
                        fieldWithContentAttribute.SetValue(assetInstance, contentObject);
                    }
                }
            }
        }
    }
}
