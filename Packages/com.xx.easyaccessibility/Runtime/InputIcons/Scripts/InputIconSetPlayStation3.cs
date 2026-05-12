using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the DualShock 3 (PlayStation 3).
    /// No device-specific paths beyond the standard gamepad layout.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/PlayStation 3")]
    public class InputIconSetPlayStation3 : InputIconSetPlayStation
    {
        protected override string Prefix => "3";
        protected override string StartName => "button_start";
        protected override string SelectName => "button_select";
    }
}
