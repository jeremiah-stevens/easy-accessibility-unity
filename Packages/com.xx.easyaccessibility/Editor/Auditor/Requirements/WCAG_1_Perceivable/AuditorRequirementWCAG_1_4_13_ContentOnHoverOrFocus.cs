namespace EasyAccessibility
{
    /*TODO:
     * - find a way of evaluating all on hover/focus functionality (static analysis?)
     */
    public class AuditorRequirementWCAG_1_4_13_ContentOnHoverOrFocus : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_13_ContentOnHoverOrFocus()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,13);
            description = AuditorRequirementKeys.GetDescription(1,4,13);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,13);
            
        }
    }
}