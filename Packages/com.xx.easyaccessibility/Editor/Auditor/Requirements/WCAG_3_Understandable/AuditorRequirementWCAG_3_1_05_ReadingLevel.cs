namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; review all text in localization or dialog system for text outside of readable level
     */
    public class AuditorRequirementWCAG_3_1_05_ReadingLevel : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_1_05_ReadingLevel()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 1, 5);
            description = AuditorRequirementKeys.GetDescription(3, 1, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 1, 5);
        }
    }
}
