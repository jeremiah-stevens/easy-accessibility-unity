namespace EasyAccessibility
{
    /*TODO:
     * - Find some way to test for this
     */
    public class AuditorRequirementWCAG_2_5_06_ConcurrentInputMechanism : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_06_ConcurrentInputMechanism()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 6);
            description = AuditorRequirementKeys.GetDescription(2, 5, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 6);
        }
    }
}