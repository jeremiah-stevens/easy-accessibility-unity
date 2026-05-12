using UnityEngine;
using UnityEngine.U2D;

namespace EasyAccessibility
{
    /// <summary>
    /// A specialized InputIconSet for keyboard controls.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Keyboard")]
    public class InputIconSetKeyboard : InputIconSet
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
                "<Keyboard>/a" => Get("keyboard_a"),
                "<Keyboard>/b" => Get("keyboard_b"),
                "<Keyboard>/c" => Get("keyboard_c"),
                "<Keyboard>/d" => Get("keyboard_d"),
                "<Keyboard>/e" => Get("keyboard_e"),
                "<Keyboard>/f" => Get("keyboard_f"),
                "<Keyboard>/g" => Get("keyboard_g"),
                "<Keyboard>/h" => Get("keyboard_h"),
                "<Keyboard>/i" => Get("keyboard_i"),
                "<Keyboard>/j" => Get("keyboard_j"),
                "<Keyboard>/k" => Get("keyboard_k"),
                "<Keyboard>/l" => Get("keyboard_l"),
                "<Keyboard>/m" => Get("keyboard_m"),
                "<Keyboard>/n" => Get("keyboard_n"),
                "<Keyboard>/o" => Get("keyboard_o"),
                "<Keyboard>/p" => Get("keyboard_p"),
                "<Keyboard>/q" => Get("keyboard_q"),
                "<Keyboard>/r" => Get("keyboard_r"),
                "<Keyboard>/s" => Get("keyboard_s"),
                "<Keyboard>/t" => Get("keyboard_t"),
                "<Keyboard>/u" => Get("keyboard_u"),
                "<Keyboard>/v" => Get("keyboard_v"),
                "<Keyboard>/w" => Get("keyboard_w"),
                "<Keyboard>/x" => Get("keyboard_x"),
                "<Keyboard>/y" => Get("keyboard_y"),
                "<Keyboard>/z" => Get("keyboard_z"),

                "<Keyboard>/digit0" => Get("keyboard_0"),
                "<Keyboard>/digit1" => Get("keyboard_1"),
                "<Keyboard>/digit2" => Get("keyboard_2"),
                "<Keyboard>/digit3" => Get("keyboard_3"),
                "<Keyboard>/digit4" => Get("keyboard_4"),
                "<Keyboard>/digit5" => Get("keyboard_5"),
                "<Keyboard>/digit6" => Get("keyboard_6"),
                "<Keyboard>/digit7" => Get("keyboard_7"),
                "<Keyboard>/digit8" => Get("keyboard_8"),
                "<Keyboard>/digit9" => Get("keyboard_9"),

                "<Keyboard>/minus" => Get("keyboard_minus"),
                "<Keyboard>/equals" => Get("keyboard_equals"),
                "<Keyboard>/leftBracket" => Get("keyboard_bracket_open"),
                "<Keyboard>/rightBracket" => Get("keyboard_bracket_close"),
                "<Keyboard>/backslash" => Get("keyboard_slash_back"),
                "<Keyboard>/semicolon" => Get("keyboard_semicolon"),
                "<Keyboard>/quote" => Get("keyboard_quote"),
                "<Keyboard>/backquote" => Get("keyboard_tilde"),
                "<Keyboard>/comma" => Get("keyboard_comma"),
                "<Keyboard>/period" => Get("keyboard_period"),
                "<Keyboard>/slash" => Get("keyboard_slash_forward"),

                "<Keyboard>/space" => Get("keyboard_space"),
                "<Keyboard>/enter" => Get("keyboard_enter"),
                "<Keyboard>/backspace" => Get("keyboard_backspace"),
                "<Keyboard>/tab" => Get("keyboard_tab"),
                "<Keyboard>/insert" => Get("keyboard_insert"),
                "<Keyboard>/delete" => Get("keyboard_delete"),

                "<Keyboard>/upArrow" => Get("keyboard_arrow_up"),
                "<Keyboard>/downArrow" => Get("keyboard_arrow_down"),
                "<Keyboard>/leftArrow" => Get("keyboard_arrow_left"),
                "<Keyboard>/rightArrow" => Get("keyboard_arrow_right"),
                "<Keyboard>/home" => Get("keyboard_home"),
                "<Keyboard>/end" => Get("keyboard_end"),
                "<Keyboard>/pageUp" => Get("keyboard_page_up"),
                "<Keyboard>/pageDown" => Get("keyboard_page_down"),

                "<Keyboard>/leftShift" => Get("keyboard_shift"),
                "<Keyboard>/rightShift" => Get("keyboard_shift"),
                "<Keyboard>/leftCtrl" => Get("keyboard_ctrl"),
                "<Keyboard>/rightCtrl" => Get("keyboard_ctrl"),
                "<Keyboard>/leftAlt" => Get("keyboard_alt"),
                "<Keyboard>/rightAlt" => Get("keyboard_alt"),
                "<Keyboard>/leftMeta" => Get("keyboard_win"),
                "<Keyboard>/rightMeta" => Get("keyboard_win"),

                "<Keyboard>/escape" => Get("keyboard_escape"),
                "<Keyboard>/capsLock" => Get("keyboard_capslock"),
                "<Keyboard>/numLock" => Get("keyboard_numlock"),
                "<Keyboard>/printScreen" => Get("keyboard_printscreen"),

                "<Keyboard>/f1" => Get("keyboard_f1"),
                "<Keyboard>/f2" => Get("keyboard_f2"),
                "<Keyboard>/f3" => Get("keyboard_f3"),
                "<Keyboard>/f4" => Get("keyboard_f4"),
                "<Keyboard>/f5" => Get("keyboard_f5"),
                "<Keyboard>/f6" => Get("keyboard_f6"),
                "<Keyboard>/f7" => Get("keyboard_f7"),
                "<Keyboard>/f8" => Get("keyboard_f8"),
                "<Keyboard>/f9" => Get("keyboard_f9"),
                "<Keyboard>/f10" => Get("keyboard_f10"),
                "<Keyboard>/f11" => Get("keyboard_f11"),
                "<Keyboard>/f12" => Get("keyboard_f12"),

                "<Keyboard>/numpad0" => Get("keyboard_0"),
                "<Keyboard>/numpad1" => Get("keyboard_1"),
                "<Keyboard>/numpad2" => Get("keyboard_2"),
                "<Keyboard>/numpad3" => Get("keyboard_3"),
                "<Keyboard>/numpad4" => Get("keyboard_4"),
                "<Keyboard>/numpad5" => Get("keyboard_5"),
                "<Keyboard>/numpad6" => Get("keyboard_6"),
                "<Keyboard>/numpad7" => Get("keyboard_7"),
                "<Keyboard>/numpad8" => Get("keyboard_8"),
                "<Keyboard>/numpad9" => Get("keyboard_9"),
                "<Keyboard>/numpadPlus" => Get("keyboard_plus"),
                "<Keyboard>/numpadMinus" => Get("keyboard_minus"),
                "<Keyboard>/numpadMultiply" => Get("keyboard_multiply"),
                "<Keyboard>/numpadDivide" => Get("keyboard_divide"),
                "<Keyboard>/numpadPeriod" => Get("keyboard_period"),
                "<Keyboard>/numpadEnter" => Get("keyboard_enter"),
                "<Keyboard>/numpadEquals" => Get("keyboard_equals"),

                _ => null,
            };
        }

        private Sprite Get(string name)
        {
            return m_atlas?.GetSprite(name + suffix) ?? m_atlas?.GetSprite(name);
        }
    }
}
