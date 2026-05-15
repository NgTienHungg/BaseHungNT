using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HungNT.Database.Editor
{
    public static class GGSheetImporter
    {
        public static void ImportAll(DatabaseConfig config, Action<float, string> onProgress = null)
        {
            if (config == null)
            {
                DebugEx.LogEditorError($"{("[GGSheet]").Bold()} {"DatabaseConfig is null.".Color("red")}");
                return;
            }

            if (config.Profiles == null || config.Profiles.Count == 0)
            {
                DebugEx.LogEditorWarning($"{("[GGSheet]").Bold()} {"No SheetProfile entries on config.".Color("orange")}");
                return;
            }

            int n = config.Profiles.Count;
            onProgress?.Invoke(0f, "Starting…");

            try
            {
                for (int i = 0; i < n; i++)
                {
                    var p = config.Profiles[i];
                    float baseProgress = i / (float)n;
                    onProgress?.Invoke(baseProgress, $"Table [{i + 1}/{n}] {p.TableName}");
                    float slice = 1f / n;
                    ImportProfile(
                        config,
                        p,
                        (sub01, status) => onProgress?.Invoke(baseProgress + sub01 * slice, status));
                }

                onProgress?.Invoke(1f, "Saving assets…");
            }
            catch (Exception ex)
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"ImportAll stopped".Color("red")}: {ex.Message}");
                throw;
            }

            AssetDatabase.SaveAssets();
            GGCallbackBindings.InvokeOnImportComplete();
            DebugEx.LogEditor($"{("[GGSheet]").Bold()} {"Import finished.".Color("lime")}");
        }

        public static void ImportProfile(
            DatabaseConfig config,
            SheetProfile profile,
            Action<float, string> onWorksheetProgress = null)
        {
            if (config == null || profile == null)
                return;

            if (profile.TableAsset == null)
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"Table asset is required".Color("red")} — " +
                    $"kéo ScriptableObject bảng vào SheetProfile ({(profile.TableName ?? "?").Color("cyan")}).");
                return;
            }

            var util = new GGImportUtility();
            var binding = new GGAssetBindings(util, profile);

            if (!binding.IsValid)
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"Bindings invalid".Color("red")} for table {"'" + profile.TableName + "'".Color("cyan")} — fix errors in Console.");
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(profile.TableAsset);
            if (string.IsNullOrEmpty(assetPath))
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"Table asset has no Project path".Color("red")} — " +
                    $"{(profile.TableName ?? "?").Color("cyan")}: lưu asset trong Project trước khi import.");
                return;
            }

            var sheets = new List<DataSheet>();
            if (profile.Worksheets == null || profile.Worksheets.Length == 0)
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"No worksheets".Color("red")} on table {"'" + profile.TableName + "'".Color("cyan")}.");
                return;
            }

            int wCount = profile.Worksheets.Length;
            for (int wi = 0; wi < wCount; wi++)
            {
                var ws = profile.Worksheets[wi];
                onWorksheetProgress?.Invoke(
                    wCount > 0 ? wi / (float)wCount : 0f,
                    $"Fetching “{ws.SheetName}”…");

                try
                {
                    List<string[]> raw = GGSheetFetcher.FetchWorksheet(config, ws);
                    raw = SheetDataFilter.ApplyCommentConvention(raw);
                    var id = new WorksheetId(config.SheetId, ws.SheetName ?? "", ws.FromCol, ws.ToCol);
                    sheets.Add(new DataSheet(id, raw, 0));
                }
                catch (Exception ex)
                {
                    DebugEx.LogEditorError(
                        $"{("[GGSheet]").Bold()} {"Fetch failed".Color("red")} — table " +
                        $"{(profile.TableName ?? "?").Color("cyan")}, worksheet {(ws?.SheetName ?? "?").Color("yellow")}: {ex.Message}");
                    Debug.LogException(ex);
                    throw;
                }
            }

            onWorksheetProgress?.Invoke(1f, "Deserialize & write…");

            util.Reset(sheets, binding.PrimaryKey, profile.TableAsset);

            try
            {
                binding.Import(util);
                binding.LateImport(util);
            }
            catch (Exception ex)
            {
                DebugEx.LogEditorError(
                    $"{("[GGSheet]").Bold()} {"Import pipeline error".Color("red")} — {profile.TableName.Color("cyan")}: {ex.Message}");
                Debug.LogException(ex);
                throw;
            }
        }
    }
}
