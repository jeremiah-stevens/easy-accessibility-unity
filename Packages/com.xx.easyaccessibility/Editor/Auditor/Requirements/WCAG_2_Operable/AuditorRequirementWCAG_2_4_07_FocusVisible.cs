namespace EasyAccessibility
{
    /*TODO:
     * - Set up a way of reviewing all UI elements and ensuring they have some styling for on focus
     * - Should check for styling in 2.4.13 (ex: contrast ratio of 3:1 between states)
     */
    public class AuditorRequirementWCAG_2_4_07_FocusVisible : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_07_FocusVisible()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 7);
            description = AuditorRequirementKeys.GetDescription(2, 4, 7);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 7);
        }
    }
}
