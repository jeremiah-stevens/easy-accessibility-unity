using UnityEngine;

namespace EasyAccessibility
{
    /// <summary>
    /// InputIconSet for the Xbox 360 controller.
    /// </summary>
    [CreateAssetMenu(menuName = "Easy Accessibility/Icon Set/Xbox 360")]
    public class InputIconSetXbox360 : InputIconSetXbox
    {
        protected override string Base        => "xbox";
        protected override string StartName   => "start";
        protected override string SelectName  => "back";
    }
}
