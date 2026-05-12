namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; likely just check for all onHover events and either review code or ensure user reviews calls
     */
    public class AuditorRequirementWCAG_3_2_01_OnFocus : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_2_01_OnFocus()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 1);
            description = AuditorRequirementKeys.GetDescription(3, 2, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 1);
        }
    }
}
