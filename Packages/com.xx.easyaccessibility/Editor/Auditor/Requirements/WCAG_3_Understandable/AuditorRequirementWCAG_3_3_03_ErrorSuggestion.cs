namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; consider setting up custom interfaces for UI elements to offer tips for error input
     */
    public class AuditorRequirementWCAG_3_3_03_ErrorSuggestion : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_03_ErrorSuggestion()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 3);
            description = AuditorRequirementKeys.GetDescription(3, 3, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 3);
        }
    }
}