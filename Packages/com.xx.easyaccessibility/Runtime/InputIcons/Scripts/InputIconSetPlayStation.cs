using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// Abstract base for PlayStation controller icon sets (DS3, DS4, DS5).
    /// Handles all standard <Gamepad> paths. Subclasses supply the sprite prefix,
    /// version-specific button names, and any device-specific paths via GetDeviceSpriteName.
    /// </summary>
    public abstract class InputIconSetPlayStation : InputIconSetGamepad
    {
        [SerializeField]
        protected SpriteAtlas m_atlas;

        [SerializeField]
        protected InputIconSet m_overrides;

        [SerializeField]
        protected string suffix = "";

        /// <summary>Kenney atlas prefix, e.g. "playstation4_".</summary>
        protected abstract string Prefix { get; }

        /// <summary>Sprite name fragment for the start/options button.</summary>
        protected virtual string StartName => "start";

        /// <summary>Sprite name fragment for the select/share/create button.</summary>
        protected virtual string SelectName => "select";

        /// <summary>
        /// Override in subclasses to handle device-specific control paths
        /// (e.g. touchpad, mic button, back buttons). Return null for unrecognized paths.
        /// Checked before the shared switch statement.
        /// </summary>
        protected virtual string GetDeviceSpriteName(string controlPath) => null;

        /// <summary>
        /// Touchpad interaction paths for this controller, shown in the editor's Touchpad section.
        /// DS3 returns empty (no touchpad). DS4/DS5 override with their HID-specific paths.
        /// </summary>
        public virtual (string path, string label)[] TouchpadPaths =>
            System.Array.Empty<(string, string)>();

        public override Sprite GetIcon(string controlPath)
        {
            var overrideSprite = m_overrides?.GetIcon(controlPath);
            if (overrideSprite != null)
                return overrideSprite;

            var name = GetDeviceSpriteName(controlPath) ?? GetSharedSpriteName(controlPath);
            if (name == null)
                return null;
            return Get(name);
        }

        private string GetSharedSpriteName(string controlPath) =>
            controlPath switch
            {
                "<Gamepad>/buttonSouth" => "playstation" + "_button_cross",
                "<Gamepad>/buttonEast" => "playstation" + "_button_circle",
                "<Gamepad>/buttonNorth" => "playstation" + "_button_triangle",
                "<Gamepad>/buttonWest" => "playstation" + "_button_square",

                "<Gamepad>/leftShoulder" => "playstation" + "_trigger_l1",
                "<Gamepad>/rightShoulder" => "playstation" + "_trigger_r1",
                "<Gamepad>/leftTrigger" => "playstation" + "_trigger_l2",
                "<Gamepad>/rightTrigger" => "playstation" + "_trigger_r2",

                "<Gamepad>/start" => "playstation" + Prefix + "_" + StartName,
                "<Gamepad>/select" => "playstation" + Prefix + "_" + SelectName,

                "<Gamepad>/leftStick" => "playstation" + "_stick_l",
                "<Gamepad>/leftStick/up" => "playstation" + "_stick_l_up",
                "<Gamepad>/leftStick/down" => "playstation" + "_stick_l_down",
                "<Gamepad>/leftStick/left" => "playstation" + "_stick_l_left",
                "<Gamepad>/leftStick/right" => "playstation" + "_stick_l_right",
                "<Gamepad>/leftStickPress" => "playstation" + "_button_l3",

                "<Gamepad>/rightStick" => "playstation" + "_stick_r",
                "<Gamepad>/rightStick/up" => "playstation" + "_stick_r_up",
                "<Gamepad>/rightStick/down" => "playstation" + "_stick_r_down",
                "<Gamepad>/rightStick/left" => "playstation" + "_stick_r_left",
                "<Gamepad>/rightStick/right" => "playstation" + "_stick_r_right",
                "<Gamepad>/rightStickPress" => "playstation" + "_button_r3",

                "<Gamepad>/dpad" => "playstation" + "_dpad",
                "<Gamepad>/dpad/up" => "playstation" + "_dpad_up",
                "<Gamepad>/dpad/down" => "playstation" + "_dpad_down",
                "<Gamepad>/dpad/left" => "playstation" + "_dpad_left",
                "<Gamepad>/dpad/right" => "playstation" + "_dpad_right",

                _ => null,
            };

        private Sprite Get(string name)
        {
            return m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
        }
    }
}
