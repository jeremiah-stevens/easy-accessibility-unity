namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_4_06_ContrastEnhanced : AuditorRequirementWCAG_1_4_03_ContrastMinimum
    {
        private float contrastRatio = 7f; //7:1




        public AuditorRequirementWCAG_1_4_06_ContrastEnhanced()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_6;
            description = AuditorRequirementKeys.Description_WCAG_1_4_6;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_6;
            status = Status.None;
        }
    }
}