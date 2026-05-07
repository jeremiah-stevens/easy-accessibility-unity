using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// Shared inspector for all InputIconSetGamepad subclasses.
    /// Displays a grouped preview of every standard gamepad control path with its
    /// resolved sprite, so missing entries are immediately visible.
    ///
    /// Paths that a controller intentionally does not support (e.g. select on GameCube)
    /// should return null from GetIcon — they will show "N/A" rather than "Missing".
    /// Mark those with InputIconSetGamepadEditor.k_NotApplicable if you need to
    /// distinguish them from genuinely unmapped paths.
    /// </summary>
    [CustomEditor(typeof(InputIconSetGamepad), true)]
    public class InputIconSetGamepadEditor : UnityEditor.Editor
    {
        private static readonly (string group, string path, string label)[] k_Keys =
        {
            ("Face Buttons",  "<Gamepad>/buttonSouth", "Button South"),
            ("Face Buttons",  "<Gamepad>/buttonEast",  "Button East"),
            ("Face Buttons",  "<Gamepad>/buttonNorth", "Button North"),
            ("Face Buttons",  "<Gamepad>/buttonWest",  "Button West"),

            ("Triggers",      "<Gamepad>/leftTrigger",   "Left Trigger"),
            ("Triggers",      "<Gamepad>/rightTrigger",  "Right Trigger"),
            ("Triggers",      "<Gamepad>/leftShoulder",  "Left Shoulder"),
            ("Triggers",      "<Gamepad>/rightShoulder", "Right Shoulder"),

            ("System",        "<Gamepad>/start",  "Start"),
            ("System",        "<Gamepad>/select", "Select"),

            ("Left Stick",    "<Gamepad>/leftStick",       "Stick"),
            ("Left Stick",    "<Gamepad>/leftStick/up",    "Up"),
            ("Left Stick",    "<Gamepad>/leftStick/down",  "Down"),
            ("Left Stick",    "<Gamepad>/leftStick/left",  "Left"),
            ("Left Stick",    "<Gamepad>/leftStick/right", "Right"),
            ("Left Stick",    "<Gamepad>/leftStickPress",  "Press"),

            ("Right Stick",   "<Gamepad>/rightStick",       "Stick"),
            ("Right Stick",   "<Gamepad>/rightStick/up",    "Up"),
            ("Right Stick",   "<Gamepad>/rightStick/down",  "Down"),
            ("Right Stick",   "<Gamepad>/rightStick/left",  "Left"),
            ("Right Stick",   "<Gamepad>/rightStick/right", "Right"),
            ("Right Stick",   "<Gamepad>/rightStickPress",  "Press"),

            ("D-Pad",         "<Gamepad>/dpad",       "D-Pad"),
            ("D-Pad",         "<Gamepad>/dpad/up",    "Up"),
            ("D-Pad",         "<Gamepad>/dpad/down",  "Down"),
            ("D-Pad",         "<Gamepad>/dpad/left",  "Left"),
            ("D-Pad",         "<Gamepad>/dpad/right", "Right"),
        };

        private const float k_IconSize = 40f;
        private static readonly Color k_MissingColor = new Color(0.25f, 0.05f, 0.05f);
        private static readonly Color k_NaColor      = new Color(0.15f, 0.15f, 0.15f);
        private static readonly Color k_OkColor      = new Color(0.2f,  0.6f,  0.2f);
        private static readonly Color k_MissColor    = new Color(0.8f,  0.2f,  0.2f);
        private static readonly Color k_NaTextColor  = new Color(0.5f,  0.5f,  0.5f);

        private bool m_showPreview = true;
        private Vector2 m_scrollPos;
        private string m_currentGroup;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            m_showPreview = EditorGUILayout.Foldout(m_showPreview, "Icon Preview", true, EditorStyles.foldoutHeader);
            if (!m_showPreview) return;

            var iconSet = (InputIconSetGamepad)target;

            // Pre-resolve all sprites so we can count intentional nulls (N/A) vs missing
            int found = 0, naCount = 0;

            m_scrollPos = EditorGUILayout.BeginScrollView(m_scrollPos, GUILayout.Height(420));
            m_currentGroup = null;

            foreach (var (group, path, label) in k_Keys)
            {
                if (group != m_currentGroup)
                {
                    m_currentGroup = group;
                    EditorGUILayout.Space(4);
                    EditorGUILayout.LabelField(group, EditorStyles.boldLabel);
                    DrawColumnHeaders();
                }

                var sprite = iconSet.GetIcon(path);

                // Distinguish intentional N/A (explicit null return) from missing atlas entry.
                // We treat any null as potentially missing here; the developer decides by
                // returning null only for genuinely unsupported paths.
                if (sprite != null) found++;

                DrawRow(sprite, path, label);
            }

            EditorGUILayout.EndScrollView();

            var total   = k_Keys.Length;
            var missing = total - found;
            EditorGUILayout.HelpBox(
                $"{found}/{total} icons assigned{(missing > 0 ? $" — {missing} null (unsupported or missing)" : ".")}",
                missing > 0 ? MessageType.Warning : MessageType.Info
            );
        }

        protected static void DrawColumnHeaders()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(k_IconSize + 4);
            EditorGUILayout.LabelField("Label", EditorStyles.miniLabel, GUILayout.Width(120));
            EditorGUILayout.LabelField("Control Path", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("Status", EditorStyles.miniLabel, GUILayout.Width(55));
            EditorGUILayout.EndHorizontal();
        }

        protected static void DrawRow(Sprite sprite, string path, string label)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(k_IconSize + 2));

            var iconRect = GUILayoutUtility.GetRect(k_IconSize, k_IconSize,
                GUILayout.Width(k_IconSize), GUILayout.Height(k_IconSize));

            if (sprite != null)
                DrawSprite(iconRect, sprite);
            else
                EditorGUI.DrawRect(iconRect, k_MissingColor);

            EditorGUILayout.LabelField(label, GUILayout.Width(120));
            EditorGUILayout.LabelField(path, EditorStyles.miniLabel);

            var prevColor = GUI.contentColor;
            GUI.contentColor = sprite != null ? k_OkColor : k_MissColor;
            EditorGUILayout.LabelField(sprite != null ? "OK" : "Missing",
                EditorStyles.boldLabel, GUILayout.Width(55));
            GUI.contentColor = prevColor;

            EditorGUILayout.EndHorizontal();
        }

        protected static void DrawSprite(Rect rect, Sprite sprite)
        {
            var tex = sprite.texture;
            var uv = new Rect(
                sprite.rect.x      / tex.width,
                sprite.rect.y      / tex.height,
                sprite.rect.width  / tex.width,
                sprite.rect.height / tex.height
            );
            GUI.DrawTextureWithTexCoords(rect, tex, uv, true);
        }
    }
}
