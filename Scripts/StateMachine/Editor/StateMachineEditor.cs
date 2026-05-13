using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;

namespace HungNT.Editor
{
    /// <summary>
    /// Custom Inspector cho StateMachine.
    /// Hiển thị danh sách state đã đăng ký và highlight state hiện tại.
    /// </summary>
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : OdinEditor
    {
        private GUIStyle _activeStyle;
        private GUIStyle _inactiveStyle;
        private GUIStyle _headerStyle;
        private double _lastRepaintTime;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space(6);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("State list available in Play Mode only.", MessageType.Info);
                return;
            }

            InitStyles();

            var machine = (StateMachine)target;
            if (!machine.IsInitialized)
            {
                EditorGUILayout.HelpBox("StateMachine not initialized yet.", MessageType.Warning);
                return;
            }

            var snapshot = machine.GetDebugSnapshot();
            DrawStateList(machine, snapshot);

            if (EditorApplication.timeSinceStartup - _lastRepaintTime > 0.5)
            {
                _lastRepaintTime = EditorApplication.timeSinceStartup;
                Repaint();
            }
        }

        private void DrawStateList(StateMachine machine, IReadOnlyDictionary<Type, IState> states)
        {
            EditorGUILayout.LabelField($"States ({states.Count})", _headerStyle);

            foreach (var kvp in states)
            {
                if (kvp.Key == typeof(NullState)) continue;

                bool isCurrent = machine.IsCurrentState(kvp.Key);
                var icon = isCurrent ? "▶ " : "   ";
                var style = isCurrent ? _activeStyle : _inactiveStyle;

                EditorGUILayout.LabelField($"{icon}{kvp.Key.Name}", style);
            }
        }

        private void InitStyles()
        {
            if (_headerStyle != null) return;

            _headerStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 12 };
            _headerStyle.normal.textColor = new Color(0.88f, 0.88f, 0.88f);

            _activeStyle = new GUIStyle(EditorStyles.label) { fontSize = 11, fontStyle = FontStyle.Bold };
            _activeStyle.normal.textColor = new Color(0.4f, 1f, 0.4f);

            _inactiveStyle = new GUIStyle(EditorStyles.label) { fontSize = 11 };
            _inactiveStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        }
    }
}
