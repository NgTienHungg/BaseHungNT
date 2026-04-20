using System.IO;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public static class GitPackageInstaller
    {
        private const string GitPackagesAssetPath = "Assets/WMHungNT/Editor/GitPackageData.asset";

        [MenuItem("HungNT/Package/Open Git Package Data", false, -1)]
        public static void OpenGitPackageData()
        {
            var asset = AssetDatabase.LoadAssetAtPath<GitPackageData>(GitPackagesAssetPath);
            if (asset != null)
            {
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }
            else
            {
                Debug.LogError($"GitPackageData not found at: {GitPackagesAssetPath}");
            }
        }

        private static GitPackageData LoadData()
        {
            var data = AssetDatabase.LoadAssetAtPath<GitPackageData>(GitPackagesAssetPath);
            if (data == null)
            {
                Debug.LogError($"GitPackageData not found at: {GitPackagesAssetPath}");
            }
            return data;
        }

        private static string GetManifestPath()
        {
            return Path.Combine(Application.dataPath, "../Packages/manifest.json");
        }

        public static void OpenManifestFile()
        {
            var manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            manifestPath = Path.GetFullPath(manifestPath);

            if (File.Exists(manifestPath))
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = manifestPath,
                    UseShellExecute = true,
                    Verb = "open"
                };
                process.Start();
            }
            else
            {
                Debug.LogError($"manifest.json file not found at {manifestPath}");
            }
        }

        public static void ReloadManifest()
        {
            var data = LoadData();
            if (data == null) return;

            string manifestPath = GetManifestPath();
            if (!File.Exists(manifestPath))
            {
                Debug.LogError("manifest.json not found.");
                return;
            }

            string json = File.ReadAllText(manifestPath);
            var manifest = JObject.Parse(json);
            var deps = manifest["dependencies"] as JObject;

            if (deps == null)
            {
                Debug.LogError("'dependencies' section not found in manifest.json.");
                return;
            }

            foreach (var pkg in data.packages)
            {
                pkg.installed = deps[pkg.name] != null;
            }

            Debug.Log("Reloaded install status from manifest.json.");
        }

        public static void UpdateManifest()
        {
            var data = LoadData();
            if (data == null) return;

            string manifestPath = GetManifestPath();
            if (!File.Exists(manifestPath))
            {
                Debug.LogError("manifest.json not found.");
                return;
            }

            string json = File.ReadAllText(manifestPath);
            var manifest = JObject.Parse(json);
            var deps = manifest["dependencies"] as JObject;

            if (deps == null)
            {
                Debug.LogError("'dependencies' section not found in manifest.json.");
                return;
            }

            bool changed = false;

            foreach (var pkg in data.packages)
            {
                if (string.IsNullOrEmpty(pkg.name)) continue;

                if (pkg.installed)
                {
                    if (string.IsNullOrEmpty(pkg.url))
                    {
                        Debug.LogWarning($"Package '{pkg.name}' is marked as installed but URL is missing.");
                        continue;
                    }

                    if (deps[pkg.name]?.ToString() != pkg.url)
                    {
                        deps[pkg.name] = pkg.url;
                        Debug.Log($"Added or updated package: {pkg.name} → {pkg.url}");
                        changed = true;
                    }
                }
                else
                {
                    if (deps.Remove(pkg.name))
                    {
                        Debug.Log($"Removed package: {pkg.name}");
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                File.WriteAllText(manifestPath, manifest.ToString());
                AssetDatabase.Refresh();
                Debug.Log("manifest.json has been updated.");
            }
            else
            {
                Debug.Log("No changes were made to manifest.json.");
            }
        }
    }
}