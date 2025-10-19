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
            name = AuditorRequirementKeys.GetTitle(1,4,4);
            description = AuditorRequirementKeys.GetDescription(1,4,4);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,4);
            
        }
    }
}