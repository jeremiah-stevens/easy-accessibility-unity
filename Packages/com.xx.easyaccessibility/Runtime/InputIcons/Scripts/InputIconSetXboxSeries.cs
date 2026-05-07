using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the Xbox Series X/S controller and Xbox Elite Series 2.
    /// Adds a Share button path and Elite Series 2 paddle paths (P1–P4).
    /// Paddle and share paths return null if no atlas sprite is assigned, so this
    /// class is safe to use for standard non-Elite Series X/S controllers.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Xbox Series")]
    public class InputIconSetXboxSeries : InputIconSetXbox
    {
        protected override string Base        => "xbox";
        protected override string StartName   => "menu";
        protected override string SelectName  => "view";

        protected override string GetDeviceSpriteName(string controlPath) => controlPath switch
        {
            // Share button (Series X/S and Elite Series 2)
            // TODO: verify exact Unity Input System path for Xbox Series share button
            "<XboxGamepadWindows>/shareButton" => Base + "_button_share",

            //TODO:  Xbox Elite Series 2 back paddles (P1–P4)

            _ => null
        };
    }
}
