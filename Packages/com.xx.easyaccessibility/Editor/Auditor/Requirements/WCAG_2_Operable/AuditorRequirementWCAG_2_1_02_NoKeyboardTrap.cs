namespace EasyAccessibility
{
    /*TODO:
     * - Find some way of testing this automatically (checking for keyboard flows?)
     */
    public class AuditorRequirementWCAG_2_1_02_NoKeyboardTrap : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_1_02_NoKeyboardTrap()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 1, 2);
            description = AuditorRequirementKeys.GetDescription(2, 1, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 1, 2);
        }
    }
}
