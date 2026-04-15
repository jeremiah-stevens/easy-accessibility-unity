using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    public class RebindableAction : MonoBehaviour
    {
        public InputActionReference action;

        [HideInInspector] public string bindingId;

        [Header("Events")]
        public UnityEvent onRebindStarted;
        public UnityEvent<string> onRebindUpdated;
        public UnityEvent onRebindEnded;
        public UnityEvent onRebindReset;
        /// <summary>
        /// Event fired when the binding has been updated (binding name)
        /// </summary>
        public UnityEvent<string> onBindingUpdated;

        //state
        InputActionRebindingExtensions.RebindingOperation rebindOperation;




        public void StartRebind()
        {
            var bindingIndex = GetBindingIndex();

            if (action.action.bindings[bindingIndex].isComposite)
            {
                var firstPartIndex = bindingIndex + 1;
                if (firstPartIndex < action.action.bindings.Count && action.action.bindings[firstPartIndex].isPartOfComposite)
                    PerformRebind(action, firstPartIndex, true);
            }
            else
            {
                PerformRebind(action, bindingIndex);
            }

            onRebindStarted.Invoke();
            UpdateBindingUI();
        }

        private void PerformRebind(InputAction action, int bindingId, bool hasMoreCompositeInputs = false)
        {
            ToggleInteractivity(false);

            rebindOperation = action.PerformInteractiveRebinding(bindingId)
                .OnCancel(op =>
                {
                    ToggleInteractivity(true);
                    onRebindEnded.Invoke();
                })
                .OnComplete(op =>
                {
                    ToggleInteractivity(true);

                    if (hasMoreCompositeInputs)
                    {
                        int nextIndex = bindingId + 1;
                        if (nextIndex < action.bindings.Count && action.bindings[nextIndex].isPartOfComposite)
                        {
                            PerformRebind(action, nextIndex, nextIndex == action.bindings.Count - 1);
                        }
                    }
                    else
                    {
                        onRebindEnded.Invoke();
                        UpdateBindingUI();
                    }
                });

            onRebindUpdated.Invoke($"Listening ({rebindOperation.expectedControlType})...");

            rebindOperation.Start();
        }

        public void ResetRebind()
        {
            var bindingIndex = GetBindingIndex();

            if (action.action.bindings[bindingIndex].isComposite)
            {
                for(int i = bindingIndex + 1; i < action.action.bindings.Count; i++)
                {
                    if (!action.action.bindings[i].isPartOfComposite) continue;

                    action.action.RemoveBindingOverride(i);
                }
            }
            else
            {
                action.action.RemoveBindingOverride(bindingIndex);
            }

            onRebindReset.Invoke();
            UpdateBindingUI();
        }

        private void ToggleInteractivity(bool enabled)
        {
            if(enabled)
            {
                action.action.actionMap.Enable();
            }
            else
            {
                action.action.actionMap.Disable();
            }
        }

        private void UpdateBindingUI()
        {
            var bindingIndex = GetBindingIndex();

            var displayString = action.action.GetBindingDisplayString(bindingIndex);
            onBindingUpdated.Invoke(displayString);
        }

        private int GetBindingIndex()
        {
            var bindingGuid = new Guid(bindingId);
            return action.action.bindings.IndexOf(b => b.id == bindingGuid);
        }




        private void OnEnable()
        {
            UpdateBindingUI();
        }




        public enum NameSource
        {
            ActionName = 0,
            LocalizationKey = 1,
            Custom = 2,
        };
    }
}