namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_4_13_FocusAppearance
        : AuditorRequirementWCAG_2_4_07_FocusVisible
    {
        public AuditorRequirementWCAG_2_4_13_FocusAppearance()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 13);
            description = AuditorRequirementKeys.GetDescription(2, 4, 13);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 13);
        }
    }
}
