using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the DualSense (PlayStation 5).
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/PlayStation 5")]
    public class InputIconSetPlayStation5 : InputIconSetPlayStation
    {
        protected override string Prefix     => "5";
        protected override string StartName  => "button_options";
        protected override string SelectName => "button_create";

        protected override string GetDeviceSpriteName(string controlPath) => controlPath switch
        {
            // Touchpad
            // TODO: Touchpad swipes (only available if you have access to PlayStation SDK)
            "<DualSenseGamepadHID>/touchpadButton"     => "playstation" + Prefix + "_touchpad",
            "<DualSenseGamepadHID>/touchpadTap"        => "playstation" + Prefix + "_touchpad_press",

            // Mute/mic button
            "<DualSenseGamepadHID>/micButton" => "playstation" + Prefix + "_button_mute",

            // TODO: Back Button Attachment (L4/R4 paddles)

            _ => null
        };

        public override (string path, string label)[] TouchpadPaths => new[]
        {
            ("<DualSenseGamepadHID>/touchpadButton",     "Click"),
            ("<DualSenseGamepadHID>/touchpadTap",        "Tap"),
            ("<DualSenseGamepadHID>/micButton",   "Microphone"),
        };
    }
}
