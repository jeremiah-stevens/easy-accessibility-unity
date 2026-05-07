using System.Collections.Generic;
using UnityEditor;
using UnityEngine.InputSystem;

namespace EasyAccessibility.Editor
{
    /// <summary>
    /// Custom editor for the RebindableAction component. Provides a dropdown for selecting which binding
    /// of the InputAction to rebind.
    /// </summary>
    [CustomEditor(typeof(RebindableAction))]
    [CanEditMultipleObjects]
    public class RebindableActionEditor : UnityEditor.Editor
    {
        private void DrawBindingPicker(InputAction action)
        {
            var labels = new List<string>();
            var ids = new List<string>();

            foreach (var binding in action.bindings)
            {
                if (binding.isComposite) continue;

                var scheme = string.IsNullOrEmpty(binding.groups) ? "Any" : binding.groups.Replace(";", ", ");
                var display = binding.isPartOfComposite
                    ? $"{binding.name}: {binding.ToDisplayString()} [{scheme}]"
                    : $"{binding.ToDisplayString()} [{scheme}]";

                labels.Add(display);
                ids.Add(binding.id.ToString());
            }

            if (ids.Count == 0) return;

            var target = (RebindableAction)this.target;
            var currentIndex = ids.IndexOf(target.bindingId);
            if (currentIndex < 0) currentIndex = 0;

            var newIndex = EditorGUILayout.Popup("Binding", currentIndex, labels.ToArray());

            if (newIndex != currentIndex || string.IsNullOrEmpty(target.bindingId))
            {
                target.bindingId = ids[newIndex];
                EditorUtility.SetDirty(target);
            }
        }




        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var actionProp = serializedObject.FindProperty("m_action");
            EditorGUILayout.PropertyField(actionProp);

            var actionRef = actionProp.objectReferenceValue as InputActionReference;
            if (actionRef?.action != null)
                DrawBindingPicker(actionRef.action);

            DrawPropertiesExcluding(serializedObject, "m_action", "bindingId");

            serializedObject.ApplyModifiedProperties();
        }
    }
}