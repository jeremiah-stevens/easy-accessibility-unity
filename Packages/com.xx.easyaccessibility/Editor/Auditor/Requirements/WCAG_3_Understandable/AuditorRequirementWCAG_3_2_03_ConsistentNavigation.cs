namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; likely review UI across pages and ensure they are given the same information/order
     */
    public class AuditorRequirementWCAG_3_2_03_ConsistentNavigation : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_2_03_ConsistentNavigation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 3);
            description = AuditorRequirementKeys.GetDescription(3, 2, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 3);
        }
    }
}