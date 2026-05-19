using EasyAccessibility.DescriptiveMedia;
using UnityEditor;

namespace EasyAccessibility.DescriptiveMedia.Editor
{
    /// <summary>
    /// Base editor for <see cref="DescriptiveMediaSO"/>. Draws the shared description
    /// source fields conditionally based on <see cref="DescriptionSourceType"/>.
    /// Concrete editors should call <see cref="DrawDescriptionFields"/> after drawing
    /// their asset-specific fields.
    /// </summary>
    [CustomEditor(typeof(DescriptiveMediaSO), true)]
    public class DescriptiveMediaSOEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDescriptionFields();
            serializedObject.ApplyModifiedProperties();
        }

        protected void DrawDescriptionFields()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_priority"));

            var sourceTypeProp = serializedObject.FindProperty("_sourceType");
            EditorGUILayout.PropertyField(sourceTypeProp);

            EditorGUILayout.Space();

            var sourceType = (DescriptionSourceType)sourceTypeProp.enumValueIndex;

            switch (sourceType)
            {
                case DescriptionSourceType.String:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"));
                    break;

                case DescriptionSourceType.LocalizationKey:
#if UNITY_LOCALIZATION
                    EditorGUILayout.PropertyField(
                        serializedObject.FindProperty("_localizationTable")
                    );
                    EditorGUILayout.PropertyField(
                        serializedObject.FindProperty("_localizationKey")
                    );
#else
                    EditorGUILayout.HelpBox(
                        "LocalizationKey requires the com.unity.localization package.",
                        MessageType.Warning
                    );
#endif
                    break;

                case DescriptionSourceType.VTT:
                    EditorGUILayout.PropertyField(
                        serializedObject.FindProperty("_vttTranscript")
                    );
                    break;
            }
        }
    }
}