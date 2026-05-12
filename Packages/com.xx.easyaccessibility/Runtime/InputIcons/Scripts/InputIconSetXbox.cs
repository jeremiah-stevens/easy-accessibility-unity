using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// Abstract base for Xbox controller icon sets (360, One, Series).
    /// Handles all standard <Gamepad> paths. Subclasses supply the sprite base name,
    /// version-specific button names, and any device-specific paths (e.g. Elite paddles)
    /// via GetDeviceSpriteName.
    /// </summary>
    public abstract class InputIconSetXbox : InputIconSetGamepad
    {
        [SerializeField]
        protected SpriteAtlas m_atlas;

        [SerializeField]
        protected InputIconSet m_overrides;

        [SerializeField]
        protected string suffix = "";

        /// <summary>
        /// Kenney atlas base name, e.g. "xbox360", "xboxone", "xboxseries".
        /// Sprite names are built as Base + "_button_a", Base + "_trigger_lt", etc.
        /// </summary>
        protected abstract string Base { get; }

        /// <summary>Sprite name fragment for the start/menu button.</summary>
        protected virtual string StartName => "menu";

        /// <summary>Sprite name fragment for the back/view button.</summary>
        protected virtual string SelectName => "view";

        /// <summary>
        /// Override in subclasses to handle device-specific control paths
        /// (e.g. Elite back paddles). Return null for unrecognized paths.
        /// Checked before the shared switch statement.
        /// </summary>
        protected virtual string GetDeviceSpriteName(string controlPath) => null;

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
                "<Gamepad>/buttonSouth" => Base + "_button_a",
                "<Gamepad>/buttonEast" => Base + "_button_b",
                "<Gamepad>/buttonNorth" => Base + "_button_y",
                "<Gamepad>/buttonWest" => Base + "_button_x",

                "<Gamepad>/leftShoulder" => Base + "_lb",
                "<Gamepad>/rightShoulder" => Base + "_rb",
                "<Gamepad>/leftTrigger" => Base + "_lt",
                "<Gamepad>/rightTrigger" => Base + "_rt",

                "<Gamepad>/start" => Base + "_button_" + StartName,
                "<Gamepad>/select" => Base + "_button_" + SelectName,

                "<Gamepad>/leftStick" => Base + "_stick_l",
                "<Gamepad>/leftStick/up" => Base + "_stick_l_up",
                "<Gamepad>/leftStick/down" => Base + "_stick_l_down",
                "<Gamepad>/leftStick/left" => Base + "_stick_l_left",
                "<Gamepad>/leftStick/right" => Base + "_stick_l_right",
                "<Gamepad>/leftStickPress" => Base + "_ls",

                "<Gamepad>/rightStick" => Base + "_stick_r",
                "<Gamepad>/rightStick/up" => Base + "_stick_r_up",
                "<Gamepad>/rightStick/down" => Base + "_stick_r_down",
                "<Gamepad>/rightStick/left" => Base + "_stick_r_left",
                "<Gamepad>/rightStick/right" => Base + "_stick_r_right",
                "<Gamepad>/rightStickPress" => Base + "_rs",

                "<Gamepad>/dpad" => Base + "_dpad",
                "<Gamepad>/dpad/up" => Base + "_dpad_up",
                "<Gamepad>/dpad/down" => Base + "_dpad_down",
                "<Gamepad>/dpad/left" => Base + "_dpad_left",
                "<Gamepad>/dpad/right" => Base + "_dpad_right",

                _ => null,
            };

        protected Sprite Get(string name) =>
            m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
    }
}
