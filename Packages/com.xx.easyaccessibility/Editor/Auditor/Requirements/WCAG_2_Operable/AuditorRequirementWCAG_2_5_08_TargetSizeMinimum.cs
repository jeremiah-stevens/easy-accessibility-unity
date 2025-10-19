namespace EasyAccessibility
{
    /*TODO:
     * - Verify all input elements are 44x44 CSS pixels big (except for inline text)
     */
    public class AuditorRequirementWCAG_2_5_08_TargetSizeMinimum : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_08_TargetSizeMinimum()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 8);
            description = AuditorRequirementKeys.GetDescription(2, 5, 8);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 8);
        }
    }
}