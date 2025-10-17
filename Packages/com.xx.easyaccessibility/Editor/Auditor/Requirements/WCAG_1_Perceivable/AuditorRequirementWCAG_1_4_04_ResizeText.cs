namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to test this; static analysis by playing scenes of UI?
     * - Consider adding in a text scale setting
     */
    public class AuditorRequirementWCAG_1_4_04_ResizeText : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_04_ResizeText()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_4;
            description = AuditorRequirementKeys.Description_WCAG_1_4_4;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_4;
            status = Status.None;
        }
    }
}