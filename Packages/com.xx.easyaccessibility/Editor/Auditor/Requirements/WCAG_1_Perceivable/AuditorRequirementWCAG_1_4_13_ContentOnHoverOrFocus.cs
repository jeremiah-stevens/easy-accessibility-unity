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
            name = AuditorRequirementKeys.Title_WCAG_1_4_13;
            description = AuditorRequirementKeys.Description_WCAG_1_4_13;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_13;
            status = Status.None;
        }
    }
}