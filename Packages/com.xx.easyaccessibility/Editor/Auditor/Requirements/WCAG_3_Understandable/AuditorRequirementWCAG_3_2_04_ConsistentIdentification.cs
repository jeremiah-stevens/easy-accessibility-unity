namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; look for elements with the same accessibility values and make sure they match
     */
    public class AuditorRequirementWCAG_3_2_04_ConsistentIdentification : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_2_04_ConsistentIdentification()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 4);
            description = AuditorRequirementKeys.GetDescription(3, 2, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 4);
        }
    }
}
