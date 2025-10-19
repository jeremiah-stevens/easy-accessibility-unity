namespace EasyAccessibility
{
    /*TODO:
     * - Verify all input elements are 44x44 CSS pixels big (except for inline text)
     */
    public class AuditorRequirementWCAG_2_5_05_TargetSizeEnhanced : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_05_TargetSizeEnhanced()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 5);
            description = AuditorRequirementKeys.GetDescription(2, 5, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 5);
        }
    }
}