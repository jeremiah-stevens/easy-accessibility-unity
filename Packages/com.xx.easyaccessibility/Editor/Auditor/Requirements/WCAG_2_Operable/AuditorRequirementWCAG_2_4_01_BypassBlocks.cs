namespace EasyAccessibility
{
    /*TODO:
     * - Is this testable/relevant to games/3D content?
     */
    public class AuditorRequirementWCAG_2_4_01_BypassBlocks : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_01_BypassBlocks()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 1);
            description = AuditorRequirementKeys.GetDescription(2, 4, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 1);
        }
    }
}