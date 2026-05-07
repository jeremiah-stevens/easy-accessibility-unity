using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the DualShock 4 (PlayStation 4).
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/PlayStation 4")]
    public class InputIconSetPlayStation4 : InputIconSetPlayStation
    {
        protected override string Prefix     => "4";
        protected override string StartName  => "button_options";
        protected override string SelectName => "button_share";

        protected override string GetDeviceSpriteName(string controlPath) => controlPath switch
        {
            // Touchpad
            // TODO: Touchpad swipes (only available if you have access to PlayStation SDK)
            "<DualShock4GamepadHID>/touchpadButton"     => "playstation" + Prefix + "_touchpad",
            "<DualShock4GamepadHID>/touchpadTap"        => "playstation" + Prefix + "_touchpad_press",

            // TODO: Back Button Attachment (L4/R4 paddles)

            _ => null
        };

        public override (string path, string label)[] TouchpadPaths => new[]
        {
            ("<DualShock4GamepadHID>/touchpadButton",     "Click"),
            ("<DualShock4GamepadHID>/touchpadTap",        "Tap")
        };
    }
}
