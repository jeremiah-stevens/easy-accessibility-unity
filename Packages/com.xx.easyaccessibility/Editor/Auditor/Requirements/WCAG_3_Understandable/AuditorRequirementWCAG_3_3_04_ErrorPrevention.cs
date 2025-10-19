namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; check for input forms and suggest checks/confirmation mechanisms for each
     */
    public class AuditorRequirementWCAG_3_3_04_ErrorPrevention : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_04_ErrorPrevention()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 4);
            description = AuditorRequirementKeys.GetDescription(3, 3, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 4);
        }
    }
}