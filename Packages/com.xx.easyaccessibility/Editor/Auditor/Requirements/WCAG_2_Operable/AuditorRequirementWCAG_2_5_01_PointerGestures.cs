namespace EasyAccessibility
{
    /*TODO:
     * - Analysis of inputs to check for these
     * - Possibly adding in a simple input settings?
     */
    public class AuditorRequirementWCAG_2_5_01_PointerGestures : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_01_PointerGestures()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 1);
            description = AuditorRequirementKeys.GetDescription(2, 5, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 1);
        }
    }
}