using EasyAccessibility.DescriptiveMedia;
using UnityEditor;

namespace EasyAccessibility.DescriptiveMedia.Editor
{
    /// <summary>
    /// Custom editor for <see cref="DescriptiveAudioClipSO"/>. Draws audio-specific
    /// fields then delegates description field rendering to
    /// <see cref="DescriptiveMediaSOEditor"/>.
    /// </summary>
    [CustomEditor(typeof(DescriptiveAudioClipSO))]
    public class DescriptiveAudioClipSOEditor : DescriptiveMediaSOEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_clip"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_category"));
            DrawDescriptionFields();
            serializedObject.ApplyModifiedProperties();
        }
    }
}