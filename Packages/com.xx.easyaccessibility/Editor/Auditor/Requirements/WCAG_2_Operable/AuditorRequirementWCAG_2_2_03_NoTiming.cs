namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_2_03_NoTiming
        : AuditorRequirementWCAG_2_2_01_TimingAdjustable
    {
        public AuditorRequirementWCAG_2_2_03_NoTiming()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 3);
            description = AuditorRequirementKeys.GetDescription(2, 2, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 3);
        }
    }
}
