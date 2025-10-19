namespace EasyAccessibility
{
    /*TODO:
     * - Set up code analyzer (Roslyn?) for evaluating for references to Input scripts and recommend migration to new Input System
     * - Review Input System Asset that every action has a Keyboard input or is rebindable
     */
    public class AuditorRequirementWCAG_2_1_01_Keyboard : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_1_01_Keyboard()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2,1,1);
            description = AuditorRequirementKeys.GetDescription(2,1,1);
            referenceLink = AuditorRequirementKeys.GetUrl(2,1,1);
            status = Status.None;
        }
    }
}