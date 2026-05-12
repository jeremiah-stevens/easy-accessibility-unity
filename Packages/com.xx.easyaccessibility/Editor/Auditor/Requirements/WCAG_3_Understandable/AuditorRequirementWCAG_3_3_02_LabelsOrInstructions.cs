namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; consider setting up custom interfaces for UI elements to offer instructions or check all input fields
     */
    public class AuditorRequirementWCAG_3_3_02_LabelsOrInstructions : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_02_LabelsOrInstructions()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 2);
            description = AuditorRequirementKeys.GetDescription(3, 3, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 2);
        }
    }
}
