using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public sealed class GGImportUtility : IImportUtility
    {
        public List<DataSheet> DataSheets { get; private set; }
        public string PrimaryKey { get; private set; }
        public ScriptableObject TargetTableAsset { get; private set; }

        public void Reset(List<DataSheet> dataSheets, string primaryKey, ScriptableObject targetTableAsset)
        {
            DataSheets = dataSheets;
            PrimaryKey = primaryKey;
            TargetTableAsset = targetTableAsset;
        }

        public string BuildAssetPath(string classTitle, string worksheetName, string extension = ".asset", string assetDirectory = "")
        {
            string dir = assetDirectory;
            if (string.IsNullOrWhiteSpace(dir))
            {
                if (TargetTableAsset != null)
                {
                    string p = AssetDatabase.GetAssetPath(TargetTableAsset);
                    dir = string.IsNullOrEmpty(p)
                        ? "Assets"
                        : (Path.GetDirectoryName(p) ?? "Assets").Replace('\\', '/');
                }
                else
                {
                    dir = "Assets";
                }
            }
            else
            {
                dir = assetDirectory.Trim().Replace('\\', '/');
            }

            return Path.Combine(dir, $"{classTitle}{extension}").Replace('\\', '/');
        }

        public T FindOrCreateAsset<T>(string path) where T : ScriptableObject =>
            FindOrCreateAsset(typeof(T), path) as T;

        public ScriptableObject FindOrCreateAsset(Type type, string path)
        {
            path = path.Replace('\\', '/');
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(path, type);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance(type);
                string directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                AssetDatabase.CreateAsset(asset, path);
                EditorUtility.SetDirty(asset);
            }

            return asset as ScriptableObject;
        }

        public bool FindAssetByName<T>(string name, out T asset) where T : UnityEngine.Object
        {
            bool ok = FindAssetByName(typeof(T), name, out UnityEngine.Object obj);
            asset = obj as T;
            return ok;
        }

        public bool FindAssetByName(Type type, string name, out UnityEngine.Object asset)
        {
            string[] guids = AssetDatabase.FindAssets($"{name} t:{type.FullName}");
            if (guids == null || guids.Length <= 0)
            {
                asset = null;
                return false;
            }

            asset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guids[0]), type);
            return asset != null;
        }

        public void LogError(string error) => Debug.LogError(error);

        public void LogError(Exception error) => Debug.LogException(error);

        public void LogWarning(string warning) => Debug.LogWarning(warning);
    }
}
