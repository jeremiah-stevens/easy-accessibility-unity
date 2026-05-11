using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the Xbox One controller and Xbox One Elite (Series 1).
    /// Elite Series 1 paddle paths are included : they return null if no atlas
    /// sprite is assigned, so this class is safe to use for non-Elite controllers.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Xbox One")]
    public class InputIconSetXboxOne : InputIconSetXbox
    {
        protected override string Base        => "xbox";
        protected override string StartName   => "menu";
        protected override string SelectName  => "view";

        protected override string GetDeviceSpriteName(string controlPath) => controlPath switch
        {
            // TODO: Xbox One Elite Series 1 back paddles (P1–P4)

            _ => null
        };
    }
}
