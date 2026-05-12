namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; likely using accessibility elements
     */
    public class AuditorRequirementWCAG_2_4_09_LinkPurposeLinkOnly : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_09_LinkPurposeLinkOnly()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 9);
            description = AuditorRequirementKeys.GetDescription(2, 4, 9);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 9);
        }
    }
}
