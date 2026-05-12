using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for Nintendo Switch 2 Pro Controller.
    ///
    /// Button mapping (standard Unity Input System paths → Switch 2 face):
    ///   buttonSouth → B  |  buttonEast → A  |  buttonNorth → X  |  buttonWest → Y
    ///   leftShoulder → L   |  rightShoulder → R
    ///   leftTrigger  → SL  |  rightTrigger  → SR
    ///   start → +  |  select → -
    ///   leftStickPress → Left Stick (L)  |  rightStickPress → Right Stick (R)
    ///
    /// Note: The Switch 2 C button has no standard <Gamepad> path and is not covered here.
    /// If your project uses a device-specific binding for it, handle it via the m_overrides chain.
    ///
    /// Expected sprite names follow the pattern "switch_*" : set the suffix field
    /// to switch between style variants (e.g. "" vs "_dark").
    /// Falls back to the unsuffixed name if the suffixed sprite is not found in the atlas.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Nintendo Switch 2")]
    public class InputIconSetNintendoSwitch2 : InputIconSetGamepad
    {
        [SerializeField]
        private SpriteAtlas m_atlas;

        [SerializeField]
        private InputIconSet m_overrides;

        [SerializeField]
        private string suffix = "";

        public override Sprite GetIcon(string controlPath)
        {
            var overrideSprite = m_overrides?.GetIcon(controlPath);
            if (overrideSprite != null)
                return overrideSprite;

            return controlPath switch
            {
                // Face buttons (Nintendo physical layout)
                "<Gamepad>/buttonSouth" => Get("switch_button_b"),
                "<Gamepad>/buttonEast" => Get("switch_button_a"),
                "<Gamepad>/buttonNorth" => Get("switch_button_x"),
                "<Gamepad>/buttonWest" => Get("switch_button_y"),

                // Shoulders and triggers
                "<Gamepad>/leftShoulder" => Get("switch_button_l"),
                "<Gamepad>/rightShoulder" => Get("switch_button_r"),
                "<Gamepad>/leftTrigger" => Get("switch_button_sl"),
                "<Gamepad>/rightTrigger" => Get("switch_button_sr"),

                // System
                "<Gamepad>/start" => Get("switch_button_plus"),
                "<Gamepad>/select" => Get("switch_button_minus"),

                // Left stick
                "<Gamepad>/leftStick" => Get("switch_stick_l"),
                "<Gamepad>/leftStick/up" => Get("switch_stick_l_up"),
                "<Gamepad>/leftStick/down" => Get("switch_stick_l_down"),
                "<Gamepad>/leftStick/left" => Get("switch_stick_l_left"),
                "<Gamepad>/leftStick/right" => Get("switch_stick_l_right"),
                "<Gamepad>/leftStickPress" => Get("switch_stick_l_press"),

                // Right stick
                "<Gamepad>/rightStick" => Get("switch_stick_r"),
                "<Gamepad>/rightStick/up" => Get("switch_stick_r_up"),
                "<Gamepad>/rightStick/down" => Get("switch_stick_r_down"),
                "<Gamepad>/rightStick/left" => Get("switch_stick_r_left"),
                "<Gamepad>/rightStick/right" => Get("switch_stick_r_right"),
                "<Gamepad>/rightStickPress" => Get("switch_stick_r_press"),

                // D-Pad
                "<Gamepad>/dpad" => Get("switch_dpad"),
                "<Gamepad>/dpad/up" => Get("switch_dpad_up"),
                "<Gamepad>/dpad/down" => Get("switch_dpad_down"),
                "<Gamepad>/dpad/left" => Get("switch_dpad_left"),
                "<Gamepad>/dpad/right" => Get("switch_dpad_right"),

                _ => null,
            };
        }

        private Sprite Get(string name)
        {
            return m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
        }
    }
}
