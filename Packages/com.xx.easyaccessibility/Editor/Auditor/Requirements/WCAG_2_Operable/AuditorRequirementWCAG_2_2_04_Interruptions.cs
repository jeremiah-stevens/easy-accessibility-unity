namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate this (notifications made optional?)
     */
    public class AuditorRequirementWCAG_2_2_04_Interruptions : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_2_04_Interruptions()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 4);
            description = AuditorRequirementKeys.GetDescription(2, 2, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 4);
        }
    }
}