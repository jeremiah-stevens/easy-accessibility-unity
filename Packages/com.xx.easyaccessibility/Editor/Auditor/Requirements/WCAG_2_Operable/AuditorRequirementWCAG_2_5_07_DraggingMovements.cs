namespace EasyAccessibility
{
    /*TODO:
     * - Check for drag movements and recommend switching to/allowing for single actions
     */
    public class AuditorRequirementWCAG_2_5_07_DraggingMovements : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_07_DraggingMovements()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 7);
            description = AuditorRequirementKeys.GetDescription(2, 5, 7);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 7);
        }
    }
}