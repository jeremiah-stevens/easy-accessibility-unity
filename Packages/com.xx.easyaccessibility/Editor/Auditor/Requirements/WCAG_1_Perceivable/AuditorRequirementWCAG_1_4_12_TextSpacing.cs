namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate
     * - Add setting for allowing text spacing
     */
    public class AuditorRequirementWCAG_1_4_12_TextSpacing : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_12_TextSpacing()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_12;
            description = AuditorRequirementKeys.Description_WCAG_1_4_12;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_12;
            status = Status.None;
        }
    }
}