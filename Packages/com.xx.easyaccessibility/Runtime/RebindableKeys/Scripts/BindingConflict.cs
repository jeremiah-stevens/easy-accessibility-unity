using UnityEngine.InputSystem;

namespace EasyAccessibility
{
    /// <summary>
    /// A conflict from an attempted rebind. Contains information about the actions and bindings involved.
    /// </summary>
    public class BindingConflict
    {
        /// <summary>
        /// The action that the user attempted to rebind.
        /// </summary>
        public InputAction action;
        /// <summary>
        /// The index of the binding that the user attempted to rebind.
        /// </summary>
        public int bindingIndex;
        /// <summary>
        /// The action that conflicts with the attempted rebind.
        /// </summary>
        public InputAction conflictingAction;
        /// <summary>
        /// The index of the binding that conflicts with the attempted rebind.
        /// </summary>
        public int conflictingBindingIndex;
        /// <summary>
        /// The path of the original binding.
        /// </summary>
        public string oldPath;




        public string GetConflictDescription()
        {
            var keyName = InputControlPath.ToHumanReadableString(
                action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice
            );

            var conflictingBinding = conflictingAction.bindings[conflictingBindingIndex];
            var conflictActionName = conflictingBinding.isPartOfComposite
                ? $"{conflictingAction.name} ({conflictingBinding.name})"
                : conflictingAction.name;

            return $"Cannot bind \"{keyName}\" to \"{action.name}\", because it is already bound to \"{conflictActionName}\".";
        }
    }
}