namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; likely using accessibility elements
     */
    public class AuditorRequirementWCAG_2_4_06_HeadingsAndLabels : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_06_HeadingsAndLabels()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 6);
            description = AuditorRequirementKeys.GetDescription(2, 4, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 6);
        }
    }
}