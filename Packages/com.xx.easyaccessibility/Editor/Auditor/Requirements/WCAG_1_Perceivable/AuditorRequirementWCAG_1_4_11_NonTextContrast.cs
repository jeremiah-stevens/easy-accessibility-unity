namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate
     */
    public class AuditorRequirementWCAG_1_4_11_NonTextContrast: AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_11_NonTextContrast()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,11);
            description = AuditorRequirementKeys.GetDescription(1,4,11);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,11);
            status = Status.None;
        }
    }
}