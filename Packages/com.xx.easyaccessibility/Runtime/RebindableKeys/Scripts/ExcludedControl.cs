using System;

namespace EasyAccessibility
{
    /// <summary>
    /// A set of frequently used controls that can be excluded from rebinding to prevent conflicts with actions.
    /// </summary>
    [Flags]
    public enum ExcludedControl
    {
        None = 0,
        MousePosition = 1 << 0,
        MouseDelta = 1 << 1,
        MouseScroll = 1 << 2,
        PointerPosition = 1 << 3,
        TouchPosition = 1 << 4,
    }
}
