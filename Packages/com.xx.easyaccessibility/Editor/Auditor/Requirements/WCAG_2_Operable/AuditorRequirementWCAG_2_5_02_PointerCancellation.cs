namespace EasyAccessibility
{
    /*TODO:
     * - Check for OnPointerDown events but don't necessarily fail them for these
     * - I believe Unity has a built-in click abortion mechanism
     */
    public class AuditorRequirementWCAG_2_5_02_PointerCancellation : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_02_PointerCancellation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 2);
            description = AuditorRequirementKeys.GetDescription(2, 5, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 2);
        }
    }
}
