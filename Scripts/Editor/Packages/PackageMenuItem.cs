using System.IO;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public static class PackageMenuItem
    {
        [MenuItem("HungNT/Package/Open Packages Folder")]
        public static void OpenPackageFolderMenu()
        {
            var packageFolderPath = Path.Combine(Application.dataPath, "../Packages/");
            packageFolderPath = Path.GetFullPath(packageFolderPath);

            if (Directory.Exists(packageFolderPath))
            {
                EditorUtility.RevealInFinder(packageFolderPath);
            }
            else
            {
                Debug.LogError("Package folder not found!");
            }
        }
    }
}