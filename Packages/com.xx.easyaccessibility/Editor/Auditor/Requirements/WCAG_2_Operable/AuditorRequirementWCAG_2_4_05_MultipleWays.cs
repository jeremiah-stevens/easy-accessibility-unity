namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; may just provide functionality
     */
    public class AuditorRequirementWCAG_2_4_05_MultipleWays : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_05_MultipleWays()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 5);
            description = AuditorRequirementKeys.GetDescription(2, 4, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 5);
        }
    }
}