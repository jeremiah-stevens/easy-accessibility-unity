namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_4_12_FocusNotObscuredEnhanced : AuditorRequirementWCAG_2_4_11_FocusNotObscuredMinimum
    {
        public AuditorRequirementWCAG_2_4_12_FocusNotObscuredEnhanced()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 12);
            description = AuditorRequirementKeys.GetDescription(2, 4, 12);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 12);
        }
    }
}