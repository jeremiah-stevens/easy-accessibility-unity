using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Custom inspector for all InputIconSetPlayStation subclasses.
    /// Extends the shared gamepad preview with a Touchpad section that shows
    /// gesture sprites (click, tap, swipe directions) for DS4 and DS5.
    /// DS3 has no touchpad — the section is hidden automatically.
    /// </summary>
    [CustomEditor(typeof(InputIconSetPlayStation), true)]
    public class InputIconSetPlayStationEditor : InputIconSetGamepadEditor
    {
        private bool m_showTouchpad = true;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var ps = (InputIconSetPlayStation)target;
            var touchpadPaths = ps.TouchpadPaths;
            if (touchpadPaths.Length == 0) return;

            EditorGUILayout.Space();
            m_showTouchpad = EditorGUILayout.Foldout(m_showTouchpad, "Touchpad", true, EditorStyles.foldoutHeader);
            if (!m_showTouchpad) return;

            DrawColumnHeaders();

            int found = 0;
            foreach (var (path, label) in touchpadPaths)
            {
                var sprite = ps.GetIcon(path);
                if (sprite != null) found++;
                DrawRow(sprite, path, label);
            }

            var missing = touchpadPaths.Length - found;
            EditorGUILayout.HelpBox(
                $"{found}/{touchpadPaths.Length} touchpad icons assigned{(missing > 0 ? $" — {missing} missing" : ".")}",
                missing > 0 ? MessageType.Warning : MessageType.Info
            );
        }
    }
}
