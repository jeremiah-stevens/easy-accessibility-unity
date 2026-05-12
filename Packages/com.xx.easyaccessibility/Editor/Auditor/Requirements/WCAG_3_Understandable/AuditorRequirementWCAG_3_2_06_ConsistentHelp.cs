namespace EasyAccessibility
{
    /*TODO:
     * - Should be a subset of consistent identification if done properly
     */
    public class AuditorRequirementWCAG_3_2_06_ConsistentHelp
        : AuditorRequirementWCAG_3_2_04_ConsistentIdentification
    {
        public AuditorRequirementWCAG_3_2_06_ConsistentHelp()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 6);
            description = AuditorRequirementKeys.GetDescription(3, 2, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 6);
        }
    }
}
