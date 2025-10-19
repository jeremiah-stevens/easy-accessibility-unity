namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; grouping UI sections (ex: pause menu, health, minimap, etc.)
     */
    public class AuditorRequirementWCAG_2_4_10_SectionHeading : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_10_SectionHeading()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 10);
            description = AuditorRequirementKeys.GetDescription(2, 4, 10);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 10);
        }
    }
}