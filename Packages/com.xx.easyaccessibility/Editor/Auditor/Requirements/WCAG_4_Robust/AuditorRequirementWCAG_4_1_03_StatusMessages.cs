namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; subset of WCA 4.1.2
     */
    public class AuditorRequirementWCAG_4_1_03_StatusMessages : AuditorRequirement
    {
        public AuditorRequirementWCAG_4_1_03_StatusMessages()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(4, 1, 3);
            description = AuditorRequirementKeys.GetDescription(4, 1, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(4, 1, 3);
        }
    }
}