// using System;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace HungNT.Editor
// {
//     /// <summary>
//     /// Custom inspector cho ServiceLocator để hiển thị key gọn hơn (tên type)
//     /// và phân biệt Mono vs non-Mono services.
//     /// </summary>
//     [CustomEditor(typeof(ServiceLocator))]
//     public class ServiceLocatorEditor : UnityEditor.Editor
//     {
//         private ServiceLocator _locator;
//
//         private void OnEnable()
//         {
//             _locator = (ServiceLocator)target;
//         }
//
//         public override void OnInspectorGUI()
//         {
//             if (!Application.isPlaying)
//             {
//                 EditorGUILayout.HelpBox("Danh sách services chỉ hiển thị trong Play Mode.", MessageType.Info);
//                 return;
//             }
//
//             var snapshot = _locator.GetDebugSnapshot();
//             if (snapshot == null || snapshot.Count == 0)
//             {
//                 EditorGUILayout.HelpBox("Chưa có service nào được register.", MessageType.Info);
//                 return;
//             }
//
//             EditorGUILayout.LabelField("Registered Services", EditorStyles.boldLabel);
//             EditorGUILayout.Space(4);
//
//             using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
//             {
//                 GUILayout.Label("Key (Service Type)", GUILayout.Width(220));
//                 GUILayout.Label("Implementation", GUILayout.ExpandWidth(true));
//             }
//
//             foreach (KeyValuePair<Type, IService> kvp in snapshot)
//             {
//                 using (new EditorGUILayout.HorizontalScope())
//                 {
//                     string keyName = kvp.Key.Name;
//                     IService impl = kvp.Value;
//
//                     GUILayout.Label(keyName, GUILayout.Width(220));
//
//                     if (impl is UnityEngine.Object unityObj)
//                     {
//                         using (new EditorGUI.DisabledScope(true))
//                         {
//                             EditorGUILayout.ObjectField(unityObj, unityObj.GetType(), true);
//                         }
//                     }
//                     else
//                     {
//                         string implName = impl != null ? impl.GetType().Name : "(null)";
//                         EditorGUILayout.LabelField(implName);
//                     }
//                 }
//             }
//
//             // Tự động repaint trong Play Mode để inspector luôn cập nhật.
//             if (Application.isPlaying)
//             {
//                 Repaint();
//             }
//         }
//     }
// }
//
