using EasyAccessibility.DescriptiveMedia;
using UnityEditor;

namespace EasyAccessibility.DescriptiveMedia.Editor
{
    /// <summary>
    /// Custom editor for <see cref="DescriptiveVideoClipSO"/>. Draws the video clip
    /// field then delegates description field rendering to
    /// <see cref="DescriptiveMediaSOEditor"/>.
    /// </summary>
    [CustomEditor(typeof(DescriptiveVideoClipSO))]
    public class DescriptiveVideoClipSOEditor : DescriptiveMediaSOEditor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_clip"));
            DrawDescriptionFields();
            serializedObject.ApplyModifiedProperties();
        }
    }
}