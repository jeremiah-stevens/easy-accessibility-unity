namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_1_03_KeyboardNoException
        : AuditorRequirementWCAG_2_1_01_Keyboard
    {
        public AuditorRequirementWCAG_2_1_03_KeyboardNoException()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 1, 3);
            description = AuditorRequirementKeys.GetDescription(2, 1, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 1, 3);
        }
    }
}
