namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; may be possible if we set up tabbing order functionality
     */
    public class AuditorRequirementWCAG_2_4_03_FocusOrder : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_03_FocusOrder()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 3);
            description = AuditorRequirementKeys.GetDescription(2, 4, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 3);
        }
    }
}