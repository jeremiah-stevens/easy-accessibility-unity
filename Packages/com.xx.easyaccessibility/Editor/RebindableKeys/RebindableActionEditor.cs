using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    [CustomEditor(typeof(RebindableAction))]
    public class RebindableActionEditor : Editor
    {
        SerializedProperty propertyAction;
        SerializedProperty propertyBindingId;

        GUIContent[] bindingOptions;
        string[] bindingOptionValues;
        int bindingOptionSelection;




        private void RefreshBindings()
        {
            var inputActionReference = (InputActionReference)propertyAction.objectReferenceValue;
            var action = inputActionReference?.action;

            Debug.Log(action);
            if(action == null)
            {
                bindingOptions = new GUIContent[0];
                bindingOptionValues = new string[0];
                bindingOptionSelection = -1;
                return;
            }

            bindingOptions = new GUIContent[action.bindings.Count];
            bindingOptionValues = new string[action.bindings.Count];
            bindingOptionSelection = -1;

            var curr = propertyBindingId.stringValue;
            for(int i = 0; i < action.bindings.Count; i++)
            {
                var currBinding = action.bindings[i];
                var currId = currBinding.id.ToString();
                var currIsGroup = !string.IsNullOrEmpty(currBinding.groups);

                if(currIsGroup)
                {
                    //TODO: append 
                }

                var displayString = action.GetBindingDisplayString(i);
                displayString = displayString.Replace('/', '\\');

                bindingOptions[i] = new GUIContent(displayString);
                bindingOptionValues[i] = currId;

                Debug.Log($"binding: {currBinding}, value: {bindingOptionValues[i]}");

                if (curr == currId)
                    bindingOptionSelection = i;
            }
        }




        protected void OnEnable()
        {
            propertyAction = serializedObject.FindProperty("action");
            propertyBindingId = serializedObject.FindProperty("bindingId");

            RefreshBindings();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();

            var bindingSelection = EditorGUILayout.Popup(new GUIContent("Binding"), bindingOptionSelection, bindingOptions);
            if(bindingSelection != bindingOptionSelection) //new selection
            {
                propertyBindingId.stringValue = bindingOptionValues[bindingSelection];
                bindingOptionSelection = bindingSelection;
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                RefreshBindings();
            }
        }
    }
}