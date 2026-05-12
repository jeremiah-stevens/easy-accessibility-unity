using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// A ScriptableObject that holds a set of icons for input actions, which can be used by the RebindableInputManager to
    /// display appropriate icons for bound actions.
    /// </summary>
    public abstract class InputIconSet : ScriptableObject
    {
        public abstract Sprite GetIcon(string controlPath);
    }
}
