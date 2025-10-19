namespace EasyAccessibility
{
    /*TODO:
     * - Find way of evaluating this automatically
     */
    public class AuditorRequirementWCAG_2_2_02_PauseStopHide : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_2_02_PauseStopHide()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 2);
            description = AuditorRequirementKeys.GetDescription(2, 2, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 2);
        }
    }
}