namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate this (session interface?)
     */
    public class AuditorRequirementWCAG_2_2_05_Reauthenticating : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_2_05_Reauthenticating()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 5);
            description = AuditorRequirementKeys.GetDescription(2, 2, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 5);
        }
    }
}
