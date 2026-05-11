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
        private Sprite m_lastIcon;

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

        /// <summary>
        /// Gets the icon sprite for the current binding from the RebindableInputManager's active icon set.
        /// Returns null if no icon set is assigned or no icon is mapped for this binding.
        /// </summary>
        public Sprite BindingIcon
        {
            get
            {
                if (!RebindableInputManager.IsInitialized || RebindableInputManager.Instance.iconSet == null)
                    return null;
                var bindingIndex = GetBindingIndex();
                if (bindingIndex < 0) return null;
                var path = m_resolvedAction.bindings[bindingIndex].effectivePath;
                return RebindableInputManager.Instance.iconSet.GetIcon(path);
            }
        }

        [Header("Events")]
        public UnityEvent onRebindStarted = new();
        public UnityEvent onRebindCompleted = new();
        public UnityEvent onRebindCancelled = new();
        public UnityEvent onRebindReset = new();
        public UnityEvent<string> onDisplayStringChanged = new();
        public UnityEvent<Sprite> onIconChanged = new();




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
            onDisplayStringChanged.Invoke(BindingDisplayString);

            var icon = BindingIcon;
            if (icon == m_lastIcon) return;
            m_lastIcon = icon;
            onIconChanged.Invoke(icon);
        }

        private int GetBindingIndex()
        {
            if (string.IsNullOrEmpty(bindingId)) return -1;
            var bindingGuid = new Guid(bindingId);
            return m_action.action.bindings.IndexOf(b => b.id == bindingGuid);
        }

        private void HandleDeviceChanged(InputDevice device)
        {
            var bindingIndex = GetBindingIndex();
            if (bindingIndex < 0) return;

            var path = m_resolvedAction.bindings[bindingIndex].effectivePath;
            if (string.IsNullOrEmpty(path)) return;

            var layoutEnd = path.IndexOf('>');
            if (layoutEnd > 0)
            {
                var layoutName = path[1..layoutEnd];
                if (!InputSystem.IsFirstLayoutBasedOnSecond(device.layout, layoutName))
                    return;
            }

            RefreshDisplayString();
        }




        public void OnEnable()
        {
            m_resolvedAction = m_action?.action;
            
            if(!RebindableInputManager.IsInitialized) return;
            RebindableInputManager.Instance.OnBindingChanged += HandleBindingChanged;
            RebindableInputManager.Instance.onBindingCancelled.AddListener(HandleRebindCancelled);
            RebindableInputManager.Instance.OnDeviceChanged += HandleDeviceChanged;
            RefreshDisplayString();
        }

        public void OnDisable()
        {
            if(!RebindableInputManager.IsInitialized) return;
            RebindableInputManager.Instance.OnBindingChanged -= HandleBindingChanged;
            RebindableInputManager.Instance.onBindingCancelled.RemoveListener(HandleRebindCancelled);
            RebindableInputManager.Instance.OnDeviceChanged -= HandleDeviceChanged;
        }
    }
}