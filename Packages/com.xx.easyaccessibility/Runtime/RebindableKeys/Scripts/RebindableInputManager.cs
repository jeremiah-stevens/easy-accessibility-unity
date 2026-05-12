using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace EasyAccessibility
{
    /// <summary>
    /// Manager for handling rebinding of input actions at runtime.
    /// </summary>
    public class RebindableInputManager : MonoBehaviour
    {
        private static RebindableInputManager m_instance;
        public static RebindableInputManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject("RebindableInputManager");
                    m_instance = go.AddComponent<RebindableInputManager>();
                }
                return m_instance;
            }
        }
        public static bool IsInitialized => m_instance != null;

        [Header("Bindings")]
        InputActionRebindingExtensions.RebindingOperation m_currentRebindOperation;

        [Header("Events")]
        /// <summary>
        /// Event fired when a rebind is cancelled by the user (e.g. by pressing the cancel button or by timing out).
        /// </summary>
        public UnityEvent onBindingCancelled = new UnityEvent();

        /// <summary>
        /// Event fired when a rebind conflicts with an existing key (ex: two actions mapped to the space bar). If a
        /// listener is registered for this event, the binding will be rejected and the old binding will be restored.
        /// Otherwise, the new binding will be accepted.
        /// </summary>
        public UnityEvent onBindingConflict = new UnityEvent();

        /// <summary>
        /// Event fired when a binding is changed. Provides the InputAction and the index of the binding that was changed.
        /// If the action is null, the binding was fully reset from ResetAllBindings.
        /// </summary>
        public UnityEvent onBindingChanged = new UnityEvent();

        /// <summary>
        /// Event fired when the user changes device (e.g. from keyboard to gamepad). This can be used to update input icons
        /// or other UI elements that reflect the current input device.
        /// </summary>
        public event Action<InputDevice> OnDeviceChanged;

        /// <summary>
        /// Event fired when a binding is changed. Provides the InputAction and the index of the binding that was changed.
        /// If the action is null, the binding was fully reset from ResetAllBindings.
        /// </summary>
        /// <remarks>Used for internal operations</remark>
        public event Action<InputAction, int> OnBindingChanged;

        /// <summary>
        /// The current binding conflict object.
        /// </summary>
        public BindingConflict CurrentConflict { get; private set; }

        [Header("Icons")]
        ///<summary>
        /// The current icon set being used.
        ///</summary>
        public InputIconSet iconSet;

        /// <summary>
        /// The last used device for this input system. This is used to determine if a device change has occurred.
        /// </summary>
        public InputDevice LastUsedDevice { get; private set; }

        [Header("Actions")]
        [Tooltip(
            "The InputActionAsset whose bindings are managed. Leave empty to use the project-wide asset from Input System settings."
        )]
        public InputActionAsset actionsAsset;

        [Header("Excluded Paths")]
        ///<summary>
        /// Inputs to exclude when listening for rebinds.
        ///</summary>
        public ExcludedControl excludedControls =
            ExcludedControl.MousePosition
            | ExcludedControl.MouseDelta
            | ExcludedControl.PointerPosition;

        /// <summary>
        /// Additional inputs to exclude when listening for rebinds, as input paths.
        /// </summary>
        public string[] additionalExcludedPaths = new string[] { };

        /// <summary>
        /// Starts a rebind on the given InputAction and binding index. The binding index can be found in the InputAction's
        /// bindings list.
        /// </summary>
        /// <param name="input">Input action to rebind</param>
        /// <param name="bindingIndex">Index of the binding to rebind</param>
        public void StartRebind(InputAction input, int bindingIndex)
        {
            CancelRebind();

            //store the prior keybind, in case we want to reject the new one
            var oldPath = input.bindings[bindingIndex].effectivePath;

            //disable our current input action map so we don't interfere with gameplay
            input.actionMap.Disable();

            //initialize the rebind operation
            var operation = input.PerformInteractiveRebinding(bindingIndex);

            //exclude the controls we don't want the user to be able to bind
            foreach (var path in GetExcludedPaths())
                operation.WithControlsExcluding(path);

            //if part of a composite, make sure we keep the expected structure
            if (input.bindings[bindingIndex].isPartOfComposite)
            {
                var path = input.bindings[bindingIndex].effectivePath;
                InputControl control = null;

                if (!string.IsNullOrEmpty(path))
                {
                    control = InputSystem.FindControl(path);
                }
                else
                {
                    // Infer control type from a sibling composite part
                    for (int i = 0; i < input.bindings.Count; i++)
                    {
                        if (i == bindingIndex || !input.bindings[i].isPartOfComposite)
                            continue;
                        var siblingPath = input.bindings[i].effectivePath;
                        if (!string.IsNullOrEmpty(siblingPath))
                        {
                            control = InputSystem.FindControl(siblingPath);
                            if (control != null)
                                break;
                        }
                    }
                }

                if (control is ButtonControl)
                    operation.WithExpectedControlType("Button");
                else if (control is AxisControl)
                    operation.WithExpectedControlType("Axis");
                else
                    operation.WithExpectedControlType("Button");
            }

            //perform the rebind and handle the results
            m_currentRebindOperation = operation
                .OnCancel(op =>
                {
                    input.actionMap.Enable();
                    m_currentRebindOperation.Dispose();
                    m_currentRebindOperation = null;
                    onBindingCancelled.Invoke();
                })
                .OnComplete(op =>
                {
                    input.actionMap.Enable();
                    m_currentRebindOperation.Dispose();
                    m_currentRebindOperation = null;

                    //check for a conflict
                    if (CheckForConflict(input, bindingIndex, oldPath))
                        return;

                    SaveBindings();
                    OnBindingChanged?.Invoke(input, bindingIndex);
                })
                .Start();
        }

        private InputActionAsset GetActions() =>
            actionsAsset != null ? actionsAsset : InputSystem.actions;

        private IEnumerable<string> GetExcludedPaths()
        {
            if (excludedControls.HasFlag(ExcludedControl.MousePosition))
                yield return "<Mouse>/position";
            if (excludedControls.HasFlag(ExcludedControl.MouseDelta))
                yield return "<Mouse>/delta";
            if (excludedControls.HasFlag(ExcludedControl.MouseScroll))
                yield return "<Mouse>/scroll";
            if (excludedControls.HasFlag(ExcludedControl.PointerPosition))
                yield return "<Pointer>/position";
            if (excludedControls.HasFlag(ExcludedControl.TouchPosition))
                yield return "<Touchscreen>/touch*/position";

            //adds synthetics controls to the exclusion set
            yield return "<Keyboard>/anyKey";

            foreach (var path in additionalExcludedPaths)
                yield return path;
        }

        #region Conflict Resolution

        /// <summary>
        /// Resolves the given conflict by rejecting the new binding and restoring the old one.
        /// </summary>
        /// <param name="c">Conflict to resolve</param>
        /// <example>If Action A is bound to 1 and Action B is bound to 2, and the user rebinds Action A to 2, then Action A will be rebound back to 1.</example>
        public void ResolveConflictBlock(BindingConflict c)
        {
            ResetBinding(c.action, c.bindingIndex);
            SaveBindings();
        }

        /// <summary>
        /// Resolves the given conflict by clearing the value of the conflicting keybind.
        /// </summary>
        /// <param name="c">Conflict to resolve</param>
        /// <example>If Action A is bound to 1 and Action B is bound to 2, and the user rebinds Action A to 2, then Action B will have its binding cleared.</example>
        public void ResolveConflictClear(BindingConflict c)
        {
            var path = c.conflictingAction.bindings[c.conflictingBindingIndex].effectivePath;
            c.conflictingAction.ApplyBindingOverride(c.conflictingBindingIndex, "");
            c.action.ApplyBindingOverride(c.bindingIndex, path);
            SaveBindings();
            OnBindingChanged?.Invoke(c.conflictingAction, c.conflictingBindingIndex);
            OnBindingChanged?.Invoke(c.action, c.bindingIndex);
        }

        /// <summary>
        /// Resolves the given conflict by swapping the two bindings.
        /// </summary>
        /// <param name="c">Conflict to resolve</param>
        /// <example>If Action A is bound to 1 and Action B is bound to 2, and the user rebinds Action A to 2, then Action B will be rebound to 1.</example>
        public void ResolveConflictSwap(BindingConflict c)
        {
            var path = c.conflictingAction.bindings[c.conflictingBindingIndex].effectivePath;
            c.conflictingAction.ApplyBindingOverride(c.conflictingBindingIndex, c.oldPath);
            c.action.ApplyBindingOverride(c.bindingIndex, path);
            SaveBindings();
            OnBindingChanged?.Invoke(c.conflictingAction, c.conflictingBindingIndex);
            OnBindingChanged?.Invoke(c.action, c.bindingIndex);
        }

        private bool CheckForConflict(InputAction action, int bindingIndex, string oldPath)
        {
            var conflict = FindConflict(action, bindingIndex);
            if (conflict.HasValue)
            {
                CurrentConflict = new BindingConflict()
                {
                    action = action,
                    bindingIndex = bindingIndex,
                    conflictingAction = conflict.Value.action,
                    conflictingBindingIndex = conflict.Value.bindingIndex,
                    oldPath = oldPath,
                };

                onBindingConflict.Invoke();
                return true;
            }
            return false;
        }

        private (InputAction action, int bindingIndex)? FindConflict(
            InputAction reboundAction,
            int reboundIndex
        )
        {
            var newPath = reboundAction.bindings[reboundIndex].effectivePath;
            foreach (var action in reboundAction.actionMap.actions)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    if (action == reboundAction && i == reboundIndex)
                        continue;
                    if (action.bindings[i].effectivePath == newPath)
                        return (action, i);
                }
            }
            return null;
        }

        #endregion


        /// <summary>
        /// Cancels the rebind.
        /// </summary>
        public void CancelRebind()
        {
            m_currentRebindOperation?.Cancel();
        }

        /// <summary>
        /// Resets the binding for the given InputAction and binding index to its default value. The binding index can be found
        /// in the InputAction's bindings list.
        /// </summary>
        /// <param name="input">Input action for which to reset the binding</param>
        /// <param name="bindingIndex">Index of the binding to reset</param>
        public void ResetBinding(InputAction input, int bindingIndex)
        {
            if (input == null || bindingIndex < 0 || bindingIndex >= input.bindings.Count)
            {
                Debug.LogWarning("Invalid input or binding index provided for ResetBinding.");
                return;
            }

            input.RemoveBindingOverride(bindingIndex);
            OnBindingChanged?.Invoke(input, bindingIndex);
        }

        /// <summary>
        /// Resets all bindings for all actions to their default values.
        /// </summary>
        public void ResetAllBindings()
        {
            var asset = GetActions();
            asset.RemoveAllBindingOverrides();

            //invoke the binding changed event with null action and -1 index to indicate a full reset
            OnBindingChanged?.Invoke(null, -1);
        }

        /// <summary>
        /// Saves the current bindings to the AccessibilitySettings asset. This should be called whenever a binding is changed or
        /// reset to persist the changes.
        /// </summary>
        public void SaveBindings()
        {
            var asset = GetActions();
            if (asset == null)
            {
                Debug.LogError(
                    "No default InputActionAsset configured. Set one in Project Settings → Input System."
                );
                return;
            }

            var rebinds = asset.SaveBindingOverridesAsJson();
            AccessibilitySettings.Instance.rebindableKeys = rebinds;
            AccessibilitySettings.Instance.Save();
        }

        /// <summary>
        /// Loads the bindings from the AccessibilitySettings asset and applies them to the Input System.
        /// </summary>
        public void LoadBindings()
        {
            var asset = GetActions();
            if (asset == null)
            {
                Debug.LogError(
                    "No default InputActionAsset configured. Set one in Project Settings → Input System."
                );
                return;
            }
            if (string.IsNullOrEmpty(AccessibilitySettings.Instance.rebindableKeys))
            {
                Debug.Log("No rebinds found in settings, skipping load.");
                return;
            }

            asset.LoadBindingOverridesFromJson(AccessibilitySettings.Instance.rebindableKeys);
        }

        private void HandleActionChange(object obj, InputActionChange change)
        {
            if (change != InputActionChange.ActionPerformed)
                return;
            var device = ((InputAction)obj).activeControl?.device;
            if (device == null || device == LastUsedDevice)
                return;
            LastUsedDevice = device;
            OnDeviceChanged?.Invoke(device);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AutoInitialize()
        {
            if (m_instance != null)
                return;
            _ = Instance; // creates the object + triggers Awake → LoadBindings
        }

        void Awake()
        {
            //Singleton behavior
            if (m_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            m_instance = this;
            if (Application.isPlaying)
                DontDestroyOnLoad(gameObject);

            OnBindingChanged += (a, i) => onBindingChanged?.Invoke();
            InputSystem.onActionChange += HandleActionChange;

            if (iconSet == null)
                Debug.LogWarning(
                    "RebindableInputManager: No icon set assigned. Input icons will not be shown.",
                    this
                );

            LoadBindings();
        }

        void OnDestroy()
        {
            if (m_instance == this)
                m_instance = null;
            CancelRebind();

            InputSystem.onActionChange -= HandleActionChange;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (iconSet == null)
                Debug.LogWarning(
                    "RebindableInputManager: iconSet is not assigned. Input icons will not be shown.",
                    this
                );
        }
#endif
    }
}
