namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate this (session interface?); recommend storing for 24 hours
     */
    public class AuditorRequirementWCAG_2_2_06_Timeouts : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_2_06_Timeouts()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 6);
            description = AuditorRequirementKeys.GetDescription(2, 2, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 6);
        }
    }
}
