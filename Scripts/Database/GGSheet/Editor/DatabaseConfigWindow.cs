using System;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public class DatabaseConfigWindow : OdinEditorWindow
    {
        /// <summary>Tham số cho Unity <c>Resources.Load</c> (không có .asset), ví dụ asset tại <c>Assets/.../Resources/Configs/DatabaseConfig.asset</c>.</summary>
        private const string DefaultConfigResourcesPath = "Configs/DatabaseConfig";

        private const string DefaultConfigAssetPath = "Assets/Game/Resources/Configs/DatabaseConfig.asset";

        [MenuItem("HungNT/Sheet Database %#h")]
        private static void OpenWindow()
        {
            var w = GetWindow<DatabaseConfigWindow>();
            w.titleContent = new GUIContent("GGSheet DB");
            w.position = new Rect(w.position.xMin, w.position.yMin, 720, 780);
            w.TryAssignDefaultConfigIfEmpty();
        }

        private float _importProgress = -1f;
        private string _importStatus = "";

        [OnInspectorGUI, PropertyOrder(-200)]
        private void DrawWindowImportProgress()
        {
            if (_importProgress < 0f)
                return;

            var r = EditorGUILayout.GetControlRect(false, 22f);
            EditorGUI.ProgressBar(r, Mathf.Clamp01(_importProgress), _importStatus ?? "");
        }

        private void ReportImportProgress(float value, string status)
        {
            _importProgress = Mathf.Clamp01(value);
            _importStatus = status ?? "";
            Repaint();
        }

        private void ClearImportProgress()
        {
            _importProgress = -1f;
            _importStatus = "";
            Repaint();
        }

        [Title("Database config asset")]
        [SerializeField]
        [AssetSelector(Paths = "Assets/")]
        [InlineEditor(Expanded = true)]
        [PropertyOrder(-10)]
        private DatabaseConfig _config;

        [Title("Actions")]
        [PropertyOrder(0)]
        [GUIColor(0.22f, 0.72f, 0.36f)]
        [Button(ButtonSizes.Large)]
        [EnableIf("@_config != null")]
        private void ImportAllTables()
        {
            RunImport(() => GGSheetImporter.ImportAll(_config, ReportImportProgress));
        }

        [PropertyOrder(40)]
        [OnInspectorGUI]
        private void DrawPerTableImportGrid()
        {
            if (_config == null || _config.Profiles == null || _config.Profiles.Count == 0)
                return;

            GUILayout.Space(10f);
            EditorGUILayout.LabelField("Import từng bảng", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("Mỗi hàng = một SheetProfile trong config (bảng dưới giống bố cục bảng).", MessageType.None);
            SirenixEditorGUI.BeginBox();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("#", EditorStyles.miniBoldLabel, GUILayout.Width(28));
            EditorGUILayout.LabelField("Table Name", EditorStyles.miniBoldLabel, GUILayout.Width(130));
            EditorGUILayout.LabelField("Table Type", EditorStyles.miniBoldLabel, GUILayout.MinWidth(180));
            EditorGUILayout.LabelField("", GUILayout.Width(72));
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < _config.Profiles.Count; i++)
            {
                var p = _config.Profiles[i];
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField($"{i}", GUILayout.Width(28));
                EditorGUILayout.SelectableLabel(p.TableName ?? "—", GUILayout.Height(EditorGUIUtility.singleLineHeight + 2), GUILayout.Width(126));
                EditorGUILayout.SelectableLabel(TableAssetTypeColumn(p), GUILayout.Height(EditorGUIUtility.singleLineHeight + 2), GUILayout.MinWidth(176));

                var bg = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.28f, 0.78f, 0.42f);
                if (GUILayout.Button("Import", GUILayout.Height(22), GUILayout.Width(68)))
                {
                    var profile = p;
                    RunImport(() =>
                    {
                        GGSheetImporter.ImportProfile(_config, profile, ReportImportProgress);
                        AssetDatabase.SaveAssets();
                        GGCallbackBindings.InvokeOnImportComplete();
                    });
                }

                GUI.backgroundColor = bg;
                EditorGUILayout.EndHorizontal();
            }

            SirenixEditorGUI.EndBox();
        }

        private void RunImport(Action import)
        {
            if (_config == null || import == null)
                return;

            ReportImportProgress(0f, "Starting…");
            try
            {
                import();
            }
            finally
            {
                ClearImportProgress();
            }
        }

        private static string TableAssetTypeColumn(SheetProfile p)
        {
            if (p?.TableAsset == null)
                return "—";
            Type t = p.TableAsset.GetType();
            return t.FullName ?? t.Name;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            TryAssignDefaultConfigIfEmpty();
        }

        private void TryAssignDefaultConfigIfEmpty()
        {
            if (_config != null)
                return;

            _config = Resources.Load<DatabaseConfig>(DefaultConfigResourcesPath);
            if (_config == null && !string.IsNullOrEmpty(DefaultConfigAssetPath))
                _config = AssetDatabase.LoadAssetAtPath<DatabaseConfig>(DefaultConfigAssetPath);
        }
    }
}

