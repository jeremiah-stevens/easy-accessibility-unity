using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    [CustomEditor(typeof(InputIconSetKeyboard))]
    public class InputIconSetKeyboardEditor : UnityEditor.Editor
    {
        private static readonly (string group, string path, string label)[] k_Keys =
        {
            ("Letters",   "<Keyboard>/a", "A"), ("Letters", "<Keyboard>/b", "B"), ("Letters", "<Keyboard>/c", "C"),
            ("Letters",   "<Keyboard>/d", "D"), ("Letters", "<Keyboard>/e", "E"), ("Letters", "<Keyboard>/f", "F"),
            ("Letters",   "<Keyboard>/g", "G"), ("Letters", "<Keyboard>/h", "H"), ("Letters", "<Keyboard>/i", "I"),
            ("Letters",   "<Keyboard>/j", "J"), ("Letters", "<Keyboard>/k", "K"), ("Letters", "<Keyboard>/l", "L"),
            ("Letters",   "<Keyboard>/m", "M"), ("Letters", "<Keyboard>/n", "N"), ("Letters", "<Keyboard>/o", "O"),
            ("Letters",   "<Keyboard>/p", "P"), ("Letters", "<Keyboard>/q", "Q"), ("Letters", "<Keyboard>/r", "R"),
            ("Letters",   "<Keyboard>/s", "S"), ("Letters", "<Keyboard>/t", "T"), ("Letters", "<Keyboard>/u", "U"),
            ("Letters",   "<Keyboard>/v", "V"), ("Letters", "<Keyboard>/w", "W"), ("Letters", "<Keyboard>/x", "X"),
            ("Letters",   "<Keyboard>/y", "Y"), ("Letters", "<Keyboard>/z", "Z"),

            ("Digits",    "<Keyboard>/digit0", "0"), ("Digits", "<Keyboard>/digit1", "1"),
            ("Digits",    "<Keyboard>/digit2", "2"), ("Digits", "<Keyboard>/digit3", "3"),
            ("Digits",    "<Keyboard>/digit4", "4"), ("Digits", "<Keyboard>/digit5", "5"),
            ("Digits",    "<Keyboard>/digit6", "6"), ("Digits", "<Keyboard>/digit7", "7"),
            ("Digits",    "<Keyboard>/digit8", "8"), ("Digits", "<Keyboard>/digit9", "9"),

            ("Punctuation", "<Keyboard>/minus",        "-"),
            ("Punctuation", "<Keyboard>/equals",       "="),
            ("Punctuation", "<Keyboard>/leftBracket",  "["),
            ("Punctuation", "<Keyboard>/rightBracket", "]"),
            ("Punctuation", "<Keyboard>/backslash",    "\\"),
            ("Punctuation", "<Keyboard>/semicolon",    ";"),
            ("Punctuation", "<Keyboard>/quote",        "'"),
            ("Punctuation", "<Keyboard>/backquote",    "`"),
            ("Punctuation", "<Keyboard>/comma",        ","),
            ("Punctuation", "<Keyboard>/period",       "."),
            ("Punctuation", "<Keyboard>/slash",        "/"),

            ("Editing", "<Keyboard>/space",     "Space"),
            ("Editing", "<Keyboard>/enter",     "Enter"),
            ("Editing", "<Keyboard>/backspace",  "Backspace"),
            ("Editing", "<Keyboard>/tab",        "Tab"),
            ("Editing", "<Keyboard>/insert",     "Insert"),
            ("Editing", "<Keyboard>/delete",     "Delete"),

            ("Navigation", "<Keyboard>/upArrow",    "Up"),
            ("Navigation", "<Keyboard>/downArrow",  "Down"),
            ("Navigation", "<Keyboard>/leftArrow",  "Left"),
            ("Navigation", "<Keyboard>/rightArrow", "Right"),
            ("Navigation", "<Keyboard>/home",       "Home"),
            ("Navigation", "<Keyboard>/end",        "End"),
            ("Navigation", "<Keyboard>/pageUp",     "Page Up"),
            ("Navigation", "<Keyboard>/pageDown",   "Page Down"),

            ("Modifiers", "<Keyboard>/leftShift",  "Left Shift"),
            ("Modifiers", "<Keyboard>/rightShift", "Right Shift"),
            ("Modifiers", "<Keyboard>/leftCtrl",   "Left Ctrl"),
            ("Modifiers", "<Keyboard>/rightCtrl",  "Right Ctrl"),
            ("Modifiers", "<Keyboard>/leftAlt",    "Left Alt"),
            ("Modifiers", "<Keyboard>/rightAlt",   "Right Alt"),
            ("Modifiers", "<Keyboard>/leftMeta",   "Left Meta"),
            ("Modifiers", "<Keyboard>/rightMeta",  "Right Meta"),

            ("System",   "<Keyboard>/escape",      "Escape"),
            ("System",   "<Keyboard>/capsLock",    "Caps Lock"),
            ("System",   "<Keyboard>/numLock",     "Num Lock"),
            ("System",   "<Keyboard>/printScreen", "Print Screen"),

            ("Function", "<Keyboard>/f1",  "F1"),  ("Function", "<Keyboard>/f2",  "F2"),
            ("Function", "<Keyboard>/f3",  "F3"),  ("Function", "<Keyboard>/f4",  "F4"),
            ("Function", "<Keyboard>/f5",  "F5"),  ("Function", "<Keyboard>/f6",  "F6"),
            ("Function", "<Keyboard>/f7",  "F7"),  ("Function", "<Keyboard>/f8",  "F8"),
            ("Function", "<Keyboard>/f9",  "F9"),  ("Function", "<Keyboard>/f10", "F10"),
            ("Function", "<Keyboard>/f11", "F11"), ("Function", "<Keyboard>/f12", "F12"),

            ("Numpad", "<Keyboard>/numpad0",        "Num 0"),
            ("Numpad", "<Keyboard>/numpad1",        "Num 1"),
            ("Numpad", "<Keyboard>/numpad2",        "Num 2"),
            ("Numpad", "<Keyboard>/numpad3",        "Num 3"),
            ("Numpad", "<Keyboard>/numpad4",        "Num 4"),
            ("Numpad", "<Keyboard>/numpad5",        "Num 5"),
            ("Numpad", "<Keyboard>/numpad6",        "Num 6"),
            ("Numpad", "<Keyboard>/numpad7",        "Num 7"),
            ("Numpad", "<Keyboard>/numpad8",        "Num 8"),
            ("Numpad", "<Keyboard>/numpad9",        "Num 9"),
            ("Numpad", "<Keyboard>/numpadPlus",     "Num +"),
            ("Numpad", "<Keyboard>/numpadMinus",    "Num -"),
            ("Numpad", "<Keyboard>/numpadMultiply", "Num *"),
            ("Numpad", "<Keyboard>/numpadDivide",   "Num /"),
            ("Numpad", "<Keyboard>/numpadPeriod",   "Num ."),
            ("Numpad", "<Keyboard>/numpadEnter",    "Num Enter"),
            ("Numpad", "<Keyboard>/numpadEquals",   "Num ="),
        };

        private const float k_IconSize = 40f;
        private static readonly Color k_MissingColor = new Color(0.25f, 0.05f, 0.05f);
        private static readonly Color k_OkColor      = new Color(0.2f,  0.6f,  0.2f);
        private static readonly Color k_MissColor    = new Color(0.8f,  0.2f,  0.2f);

        private bool m_showPreview = true;
        private Vector2 m_scrollPos;
        private string m_currentGroup;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();
            m_showPreview = EditorGUILayout.Foldout(m_showPreview, "Icon Preview", true, EditorStyles.foldoutHeader);
            if (!m_showPreview) return;

            var iconSet = (InputIconSetKeyboard)target;
            int found = 0;

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
                if (sprite != null) found++;

                DrawRow(sprite, path, label);
            }

            EditorGUILayout.EndScrollView();

            var missing = k_Keys.Length - found;
            EditorGUILayout.HelpBox(
                $"{found}/{k_Keys.Length} icons assigned{(missing > 0 ? $" : {missing} missing" : ".")}",
                missing > 0 ? MessageType.Warning : MessageType.Info
            );
        }

        private static void DrawColumnHeaders()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(k_IconSize + 4);
            EditorGUILayout.LabelField("Label", EditorStyles.miniLabel, GUILayout.Width(90));
            EditorGUILayout.LabelField("Control Path", EditorStyles.miniLabel);
            EditorGUILayout.LabelField("Status", EditorStyles.miniLabel, GUILayout.Width(55));
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawRow(Sprite sprite, string path, string label)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Height(k_IconSize + 2));

            var iconRect = GUILayoutUtility.GetRect(k_IconSize, k_IconSize,
                GUILayout.Width(k_IconSize), GUILayout.Height(k_IconSize));

            if (sprite != null)
                DrawSprite(iconRect, sprite);
            else
                EditorGUI.DrawRect(iconRect, k_MissingColor);

            EditorGUILayout.LabelField(label, GUILayout.Width(90));
            EditorGUILayout.LabelField(path, EditorStyles.miniLabel);

            var prevColor = GUI.contentColor;
            GUI.contentColor = sprite != null ? k_OkColor : k_MissColor;
            EditorGUILayout.LabelField(sprite != null ? "OK" : "Missing",
                EditorStyles.boldLabel, GUILayout.Width(55));
            GUI.contentColor = prevColor;

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSprite(Rect rect, Sprite sprite)
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
