namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_4_06_ContrastEnhanced
        : AuditorRequirementWCAG_1_4_03_ContrastMinimum
    {
        private float contrastRatio = 7f; //7:1

        public AuditorRequirementWCAG_1_4_06_ContrastEnhanced()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 4, 6);
            description = AuditorRequirementKeys.GetDescription(1, 4, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 4, 6);
        }
    }
}
