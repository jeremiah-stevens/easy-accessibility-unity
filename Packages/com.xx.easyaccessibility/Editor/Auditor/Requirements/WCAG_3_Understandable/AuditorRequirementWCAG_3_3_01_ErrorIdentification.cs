namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; consider setting up custom interfaces for UI elements that offer new error fields for issues or
     *   have users manually check all input points
     */
    public class AuditorRequirementWCAG_3_3_01_ErrorIdentification : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_01_ErrorIdentification()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 1);
            description = AuditorRequirementKeys.GetDescription(3, 3, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 1);
        }
    }
}
