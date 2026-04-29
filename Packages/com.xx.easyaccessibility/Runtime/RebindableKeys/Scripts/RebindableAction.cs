using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    /// <summary>
    /// Component for handling rebinding of a single InputAction at runtime.
    /// </summary>
    public class RebindableAction : MonoBehaviour
    {
        private InputAction m_resolvedAction;
        [SerializeField] private InputActionReference m_action;
        [HideInInspector] public string bindingId;
        public InputAction Action => m_action?.action;

        /// <summary>
        /// Gets the display name of the action, including the name of the composite part if this is a composite binding.
        /// </summary>
        /// <example>For the "Move" action with a composite binding, this would return "Move (Left Stick)."</example>
        public string ActionName
        {
            get
            {
                var bindingIndex = GetBindingIndex();
                if (bindingIndex >= 0 && m_action.action.bindings[bindingIndex].isPartOfComposite)
                    return $"{m_action.action.name} ({m_action.action.bindings[bindingIndex].name})";
                return m_action.action.name ?? string.Empty;
            }
        }

        /// <summary>
        /// Gets the display string for the current binding. This takes into account things like composite bindings and
        /// control schemes.
        /// </summary>
        /// <example>For a WASD composite binding, this would return "W" for the up binding, "A" for the left binding, etc.</example>
        public string BindingDisplayString
        {
            get
            {
                var bindingIndex = GetBindingIndex();
                if (bindingIndex < 0) return "";

                var path = m_resolvedAction.bindings[bindingIndex].effectivePath;
                var display = InputControlPath.ToHumanReadableString(
                    path,
                    InputControlPath.HumanReadableStringOptions.OmitDevice
                );

                return display;
            }
        }

        [Header("Events")]
        public UnityEvent onRebindStarted;
        public UnityEvent onRebindCompleted;
        public UnityEvent onRebindCancelled;
        public UnityEvent onRebindReset;
        public UnityEvent<string> onDisplayStringChanged;




        /// <summary>
        /// Starts the rebind operation on the InputAction.
        /// </summary>
        public void StartRebind()
        {
            var bindingIndex = GetBindingIndex();
            if (m_resolvedAction.bindings[bindingIndex].isComposite)
                bindingIndex++;

            RebindableInputManager.Instance.StartRebind(m_resolvedAction, bindingIndex);
            onRebindStarted.Invoke();
        }

        /// <summary>
        /// Resets the binding for this action to its default value.
        /// </summary>
        public void ResetBinding()
        {
            var bindingIndex = GetBindingIndex();

            if (m_resolvedAction.bindings[bindingIndex].isComposite)
            {
                for (int i = bindingIndex + 1; i < m_resolvedAction.bindings.Count; i++)
                {
                    if (!m_resolvedAction.bindings[i].isPartOfComposite) break;
                    RebindableInputManager.Instance.ResetBinding(m_resolvedAction, i);
                }
            }
            else
            {
                RebindableInputManager.Instance.ResetBinding(m_resolvedAction, bindingIndex);
            }

            onRebindReset.Invoke();
            RefreshDisplayString();
        }

        private void HandleBindingChanged(InputAction action, int bindingIndex)
        {
            if (action != null && action.id != m_resolvedAction?.id) return;
            onRebindCompleted.Invoke();
            RefreshDisplayString();
        }

        private void HandleRebindCancelled()
        {
            onRebindCancelled.Invoke();
        }

        private void RefreshDisplayString()
        {
            var display = BindingDisplayString;
            onDisplayStringChanged.Invoke(display);
        }

        private int GetBindingIndex()
        {
            if (string.IsNullOrEmpty(bindingId)) return -1;
            var bindingGuid = new Guid(bindingId);
            return m_action.action.bindings.IndexOf(b => b.id == bindingGuid);
        }




        private void OnEnable()
        {
            m_resolvedAction = m_action?.action;
            
            if(!RebindableInputManager.IsInitialized) return;
            RebindableInputManager.Instance.OnBindingChanged += HandleBindingChanged;
            RebindableInputManager.Instance.onBindingCancelled.AddListener(HandleRebindCancelled);
            RefreshDisplayString();
        }

        private void OnDisable()
        {
            if(!RebindableInputManager.IsInitialized) return;
            RebindableInputManager.Instance.OnBindingChanged -= HandleBindingChanged;
            RebindableInputManager.Instance.onBindingCancelled.RemoveListener(HandleRebindCancelled);
        }
    }
}