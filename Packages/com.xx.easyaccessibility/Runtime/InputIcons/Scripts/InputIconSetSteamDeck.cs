using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the Steam Deck.
    ///
    /// Unlike the Steam Controller, the Steam Deck has physical left/right sticks,
    /// a physical D-pad, AND two trackpads, so all standard <Gamepad> paths map
    /// to their natural physical equivalents. The trackpads and back grip buttons
    /// (L4/L5/R4/R5) have no standard <Gamepad> path : bind them via Steam Input's
    /// action set remapping or handle through m_overrides with device-specific paths.
    ///
    /// Expected sprite names follow the pattern "steamdeck_*".
    /// Falls back to the unsuffixed name if the suffixed sprite is not found in the atlas.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Steam Deck")]
    public class InputIconSetSteamDeck : InputIconSetGamepad
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
                // Face buttons (Xbox layout)
                "<Gamepad>/buttonSouth" => Get("steamdeck_button_a"),
                "<Gamepad>/buttonEast" => Get("steamdeck_button_b"),
                "<Gamepad>/buttonNorth" => Get("steamdeck_button_y"),
                "<Gamepad>/buttonWest" => Get("steamdeck_button_x"),

                // Shoulders and triggers
                "<Gamepad>/leftShoulder" => Get("steamdeck_button_l1"),
                "<Gamepad>/rightShoulder" => Get("steamdeck_button_r1"),
                "<Gamepad>/leftTrigger" => Get("steamdeck_button_l2"),
                "<Gamepad>/rightTrigger" => Get("steamdeck_button_r2"),

                // System
                "<Gamepad>/start" => Get("steamdeck_button_options"),
                "<Gamepad>/select" => Get("steamdeck_button_view"),

                // Left stick
                "<Gamepad>/leftStick" => Get("steamdeck_stick_l"),
                "<Gamepad>/leftStick/up" => Get("steamdeck_stick_l_up"),
                "<Gamepad>/leftStick/down" => Get("steamdeck_stick_l_down"),
                "<Gamepad>/leftStick/left" => Get("steamdeck_stick_l_left"),
                "<Gamepad>/leftStick/right" => Get("steamdeck_stick_l_right"),
                "<Gamepad>/leftStickPress" => Get("steamdeck_stick_l_press"),

                // Right stick
                "<Gamepad>/rightStick" => Get("steamdeck_stick_r"),
                "<Gamepad>/rightStick/up" => Get("steamdeck_stick_r_up"),
                "<Gamepad>/rightStick/down" => Get("steamdeck_stick_r_down"),
                "<Gamepad>/rightStick/left" => Get("steamdeck_stick_r_left"),
                "<Gamepad>/rightStick/right" => Get("steamdeck_stick_r_right"),
                "<Gamepad>/rightStickPress" => Get("steamdeck_stick_r_press"),

                // D-Pad
                "<Gamepad>/dpad" => Get("steamdeck_dpad"),
                "<Gamepad>/dpad/up" => Get("steamdeck_dpad_up"),
                "<Gamepad>/dpad/down" => Get("steamdeck_dpad_down"),
                "<Gamepad>/dpad/left" => Get("steamdeck_dpad_left"),
                "<Gamepad>/dpad/right" => Get("steamdeck_dpad_right"),

                // Back grip buttons (L4/L5/R4/R5)
                // TODO: verify exact Unity/Steam Input paths for Steam Deck back buttons
                "<SteamDeckGamepad>/l4" => Get("steamdeck_button_l4"),
                "<SteamDeckGamepad>/l5" => Get("steamdeck_button_l5"),
                "<SteamDeckGamepad>/r4" => Get("steamdeck_button_r4"),
                "<SteamDeckGamepad>/r5" => Get("steamdeck_button_r5"),

                _ => null,
            };
        }

        private Sprite Get(string name)
        {
            return m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
        }
    }
}
