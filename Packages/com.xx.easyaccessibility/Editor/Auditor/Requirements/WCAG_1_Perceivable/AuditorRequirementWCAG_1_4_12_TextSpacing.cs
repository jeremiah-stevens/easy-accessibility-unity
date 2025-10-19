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
            name = AuditorRequirementKeys.GetTitle(1,4,12);
            description = AuditorRequirementKeys.GetDescription(1,4,12);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,12);
            
        }
    }
}