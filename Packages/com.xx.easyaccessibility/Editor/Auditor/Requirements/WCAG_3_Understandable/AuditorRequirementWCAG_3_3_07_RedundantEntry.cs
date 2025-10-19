namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; check for same labels? Need to carve out exception for security items (ex: password)
     */
    public class AuditorRequirementWCAG_3_3_07_RedundantEntry : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_07_RedundantEntry()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 7);
            description = AuditorRequirementKeys.GetDescription(3, 3, 7);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 7);
        }
    }
}