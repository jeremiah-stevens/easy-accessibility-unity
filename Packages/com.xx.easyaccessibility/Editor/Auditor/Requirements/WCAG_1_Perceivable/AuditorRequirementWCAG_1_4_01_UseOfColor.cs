namespace EasyAccessibility
{
    /*TODO:
     * - Find way of evaluating this automatically; check for key words in localization?
     */
    public class AuditorRequirementWCAG_1_4_01_UseOfColor : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_01_UseOfColor()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_1;
            description = AuditorRequirementKeys.Description_WCAG_1_4_1;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_1;
            status = Status.None;
        }
    }
}