namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_1_04_CharacterKeyShortcuts : AuditorRequirementWCAG_2_1_01_Keyboard
    {
        public AuditorRequirementWCAG_2_1_04_CharacterKeyShortcuts()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2,1,4);
            description = AuditorRequirementKeys.GetDescription(2,1,4);
            referenceLink = AuditorRequirementKeys.GetUrl(2,1,4);
            status = Status.None;
        }
    }
}