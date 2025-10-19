namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; check for all links in text and recommend the user review?
     */
    public class AuditorRequirementWCAG_2_4_04_LinkPurposeInContext : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_04_LinkPurposeInContext()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 4);
            description = AuditorRequirementKeys.GetDescription(2, 4, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 4);
        }
    }
}