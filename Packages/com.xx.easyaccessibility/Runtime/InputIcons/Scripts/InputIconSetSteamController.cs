using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the Steam Controller.
    ///
    /// The Steam Controller has no physical right stick or D-pad. Steam Input maps them as:
    ///   rightStick / rightStickPress → Right Trackpad (joystick mode)
    ///   dpad / dpad directions       → Left Trackpad (d-pad mode)
    /// Grip buttons have no standard <Gamepad> path — bind them via Steam Input's
    /// action set remapping or handle through m_overrides with a custom path.
    ///
    /// Expected sprite names follow the pattern "steam_*".
    /// Falls back to the unsuffixed name if the suffixed sprite is not found in the atlas.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Steam Controller")]
    public class InputIconSetSteamController : InputIconSetGamepad
    {
        [SerializeField] private SpriteAtlas m_atlas;
        [SerializeField] private InputIconSet m_overrides;
        [SerializeField] private string suffix = "";

        public override Sprite GetIcon(string controlPath)
        {
            var overrideSprite = m_overrides?.GetIcon(controlPath);
            if (overrideSprite != null) return overrideSprite;

            return controlPath switch
            {
                // Face buttons
                "<Gamepad>/buttonSouth" => Get("steam_button_a"),
                "<Gamepad>/buttonEast"  => Get("steam_button_b"),
                "<Gamepad>/buttonNorth" => Get("steam_button_y"),
                "<Gamepad>/buttonWest"  => Get("steam_button_x"),

                // Shoulders and triggers
                "<Gamepad>/leftShoulder"  => Get("steam_lb"),
                "<Gamepad>/rightShoulder" => Get("steam_rb"),
                "<Gamepad>/leftTrigger"   => Get("steam_lt"),
                "<Gamepad>/rightTrigger"  => Get("steam_rt"),

                // System
                "<Gamepad>/start"  => Get("steam_button_start_icon"),
                "<Gamepad>/select" => Get("steam_button_back_icon"),

                // Left stick (physical analog stick)
                "<Gamepad>/leftStick"        => Get("steam_stick"),
                "<Gamepad>/leftStick/up"     => Get("steam_stick_up"),
                "<Gamepad>/leftStick/down"   => Get("steam_stick_down"),
                "<Gamepad>/leftStick/left"   => Get("steam_stick_left"),
                "<Gamepad>/leftStick/right"  => Get("steam_stick_right"),
                "<Gamepad>/leftStickPress"   => Get("steam_stick_l_press"),

                // Right trackpad — maps to rightStick in joystick mode
                "<Gamepad>/rightStick"       => Get("steam_pad"),
                "<Gamepad>/rightStick/up"    => Get("steam_pad_up"),
                "<Gamepad>/rightStick/down"  => Get("steam_pad_down"),
                "<Gamepad>/rightStick/left"  => Get("steam_pad_left"),
                "<Gamepad>/rightStick/right" => Get("steam_pad_right"),
                "<Gamepad>/rightStickPress"  => Get("steam_pad_center"),

                // Left trackpad — maps to dpad in d-pad mode
                "<Gamepad>/dpad"       => Get("steam_dpad"),
                "<Gamepad>/dpad/up"    => Get("steam_dpad_up"),
                "<Gamepad>/dpad/down"  => Get("steam_dpad_down"),
                "<Gamepad>/dpad/left"  => Get("steam_dpad_left"),
                "<Gamepad>/dpad/right" => Get("steam_dpad_right"),

                _ => null
            };
        }

        private Sprite Get(string name)
        {
            return m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
        }
    }
}
