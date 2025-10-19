namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate this (look for animations/flashes? If find one over threshold, recommend either turning off or softening)
     */
    public class AuditorRequirementWCAG_2_3_01_ThreeFlashesOrBelowThreshold : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_3_01_ThreeFlashesOrBelowThreshold()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 3, 1);
            description = AuditorRequirementKeys.GetDescription(2, 3, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 3, 1);
        }
    }
}