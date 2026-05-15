using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public interface IImportUtility : ILogger
    {
        List<DataSheet> DataSheets { get; }

        string PrimaryKey { get; }

        /// <summary>ScriptableObject bảng — import tự động ghi dữ liệu trực tiếp vào đây.</summary>
        ScriptableObject TargetTableAsset { get; }

        string BuildAssetPath(string classTitle, string worksheetName, string extension = ".asset", string assetDirectory = "");

        T FindOrCreateAsset<T>(string path) where T : ScriptableObject;

        ScriptableObject FindOrCreateAsset(Type type, string path);

        bool FindAssetByName<T>(string name, out T asset) where T : UnityEngine.Object;

        bool FindAssetByName(Type type, string name, out UnityEngine.Object asset);

        void Reset(List<DataSheet> dataSheets, string primaryKey, ScriptableObject targetTableAsset);
    }
}
