using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WingsMob.HungNT.Editor
{
    [CreateAssetMenu(fileName = "GitPackageData", menuName = "HungNT/Git Package Data", order = 0)]
    public class GitPackageData : ScriptableObject
    {
        [Serializable]
        [GUIColor(nameof(GetColorByCheckInstalled))]
        public class GitPackage
        {
            public string name;
            public string url;
            public bool installed;
        }

        [ListDrawerSettings(ListElementLabelName = nameof(name))]
        public List<GitPackage> packages = new();

        public Color GetColorByCheckInstalled(GitPackage pkg)
        {
            return pkg.installed ? Color.green : Color.gray;
        }

        [PropertySpace]
        [Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        [GUIColor("cyan")]
        private void OpenManifest()
        {
            GitPackageInstaller.OpenManifestFile();
        }

        [PropertySpace]
        [Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        [GUIColor("yellow")]
        private void ReloadManifest()
        {
            GitPackageInstaller.ReloadManifest();
        }

        [PropertySpace]
        [Button(ButtonSizes.Large)]
        [HorizontalGroup("Buttons")]
        [GUIColor("orange")]
        private void UpdateManifest()
        {
            GitPackageInstaller.UpdateManifest();
        }
    }
}