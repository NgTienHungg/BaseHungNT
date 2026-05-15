using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public class GGAssetBindings
    {
        public ImportType ImportType { get; }
        public string PrimaryKey { get; }

        private readonly Type _assetType;
        private MethodInfo _importMethod;
        private MethodInfo _lateImportMethod;

        public GGAssetBindings(ILogger logger, SheetProfile profile)
        {
            if (profile?.TableAsset == null)
            {
                logger.LogError("SheetProfile.TableAsset is null.");
                return;
            }

            Type assetType = profile.TableAsset.GetType();

            if (!assetType.IsAsset())
            {
                logger.LogError($"{assetType.Name} must inherit from ScriptableObject and use [ContentAsset].");
                return;
            }

            ContentAssetAttribute contentAsset = assetType.GetCustomAttribute<ContentAssetAttribute>();
            if (contentAsset == null)
            {
                logger.LogError($"{assetType.Name} needs [ContentAsset].");
                return;
            }

            _assetType = assetType;
            ImportType = contentAsset.ImportType;
            PrimaryKey = contentAsset.PrimaryKey;

            if (ImportType == ImportType.Manual || ImportType == ImportType.Both)
                SetupManual(logger);
        }

        public bool IsValid => _assetType != null && (ImportType == ImportType.Automatic || _importMethod != null);

        public void Import(IImportUtility util)
        {
            if (_assetType == null)
                return;

            if (ImportType == ImportType.Manual)
            {
                _importMethod?.Invoke(null, new object[] { util });
            }
            else if (ImportType == ImportType.Automatic)
            {
                ImportAutomaticContent(util);
            }
            else if (ImportType == ImportType.Both)
            {
                ImportAutomaticContent(util);
                _importMethod?.Invoke(null, new object[] { util });
            }
        }

        public void LateImport(IImportUtility util)
        {
            if (_assetType == null)
                return;

            if (ImportType == ImportType.Manual || ImportType == ImportType.Both)
                _lateImportMethod?.Invoke(null, new object[] { util });
        }

        private void SetupManual(ILogger logger)
        {
            _importMethod = _assetType.GetMethod("Import", BindingFlags.Public | BindingFlags.Static);
            if (_importMethod == null)
            {
                logger.LogError($"ContentAsset `{_assetType.Name}' needs public static void Import(IImportUtility util).");
            }
            else
            {
                ParameterInfo[] parameters = _importMethod.GetParameters();
                if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(IImportUtility))
                    logger.LogError($"ContentAsset `{_assetType.Name}': Import must take a single IImportUtility parameter.");
            }

            _lateImportMethod = _assetType.GetMethod("LateImport", BindingFlags.Public | BindingFlags.Static);
            if (_lateImportMethod != null)
            {
                ParameterInfo[] parameters = _lateImportMethod.GetParameters();
                if (parameters == null || parameters.Length != 1 || parameters[0].ParameterType != typeof(IImportUtility))
                    logger.LogError($"ContentAsset `{_assetType.Name}': LateImport must take a single IImportUtility parameter.");
            }
        }

        private void ImportAutomaticContent(IImportUtility util)
        {
            if (util.DataSheets == null || util.DataSheets.Count == 0)
                return;

            ScriptableObject target = util.TargetTableAsset;
            if (target == null)
            {
                Debug.LogError("[GGSheet] TableAsset chưa gán — cần kéo ScriptableObject bảng vào SheetProfile.");
                return;
            }

            if (!_assetType.IsAssignableFrom(target.GetType()))
            {
                Debug.LogError(
                    $"[GGSheet] Table asset là {target.GetType().FullName}, cần type assignable tới {_assetType.FullName}.");
                return;
            }

            string path = AssetDatabase.GetAssetPath(target);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogError("[GGSheet] Table asset chưa có path trên Project — lưu asset vào một thư mục trong Assets trước.");
                return;
            }

            SheetReader.SetValues(util, _assetType, target);
            EditorUtility.SetDirty(target);
            DebugEx.Log($"Saved: {path.Bold()}");
        }
    }
}
