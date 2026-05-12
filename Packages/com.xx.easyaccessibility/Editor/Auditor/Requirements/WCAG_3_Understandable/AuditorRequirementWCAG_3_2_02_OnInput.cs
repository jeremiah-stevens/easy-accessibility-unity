namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; likely just making sure the form doesn't change too dynamically based on input
     */
    public class AuditorRequirementWCAG_3_2_02_OnInput : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_2_02_OnInput()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 2);
            description = AuditorRequirementKeys.GetDescription(3, 2, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 2);
        }
    }
}
