using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HungNT.Editor
{
    public class SceneEditorWindow : OdinEditorWindow
    {
        [MenuItem("HungNT/Scene Editor Window")]
        private static void OpenWindow()
        {
            var window = GetWindow<SceneEditorWindow>();
            window.titleContent = new GUIContent("Scene Editor");
        }

        protected override void OnBeginDrawEditors()
        {
            base.OnBeginDrawEditors();

            DrawPlayGameButton();

            var buildScenes = EditorBuildSettings.scenes;

            // Build a dictionary mapping scene path => buildIndex (for enabled scenes)
            var pathToBuildIndex = new System.Collections.Generic.Dictionary<string, int>();
            int buildIndexCounter = 0;
            for (int i = 0; i < buildScenes.Length; i++)
            {
                if (buildScenes[i].enabled)
                {
                    pathToBuildIndex[buildScenes[i].path] = buildIndexCounter;
                    buildIndexCounter++;
                }
            }

            for (var i = 0; i < buildScenes.Length; i++)
            {
                var sceneSetting = buildScenes[i];
                var scenePath = sceneSetting.path;
                var sceneName = Path.GetFileNameWithoutExtension(scenePath);

                bool isEnabled = sceneSetting.enabled;
                string displayName = isEnabled
                    ? $"{pathToBuildIndex[scenePath]}. {sceneName}"
                    : $"_ {sceneName}";

                SirenixEditorGUI.BeginHorizontalToolbar();
                bool clicked = SirenixEditorGUI.MenuButton(i, displayName, true, null);

                GUILayout.Space(5);

                if (GUILayout.Button("Ping", GUILayout.Width(50)))
                {
                    var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                    if (sceneAsset != null)
                    {
                        EditorGUIUtility.PingObject(sceneAsset);
                        Selection.activeObject = sceneAsset;
                    }
                }

                GUILayout.Space(10);

                SirenixEditorGUI.EndHorizontalToolbar();

                if (clicked)
                {
                    OnChangeScene(scenePath);
                }
            }
        }

        private void OnChangeScene(string scenePath)
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);
        }

        private void DrawPlayGameButton()
        {
            GUILayout.Space(5);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Quick Play Game", GUILayout.Height(30)))
            {
                var buildScenes = EditorBuildSettings.scenes;
                // Find scene with buildIndex = 0 in enabled scenes
                if (buildScenes.Length > 0 && buildScenes[0].enabled)
                {
                    var scenePath = buildScenes[0].path;
                    EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                    EditorSceneManager.OpenScene(scenePath);
                    EditorApplication.isPlaying = true;
                }
                else
                {
                    EditorUtility.DisplayDialog("Quick Play Game", "Scene 0 is not enabled in Build Settings!", "OK");
                }
            }
            GUI.backgroundColor = Color.white;
            GUILayout.Space(5);
        }
    }
}