namespace EasyAccessibility
{
    /*TODO:
     * - OCR for testing images?
     */
    public class AuditorRequirementWCAG_1_4_05_ImagesOfText : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_05_ImagesOfText()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,5);
            description = AuditorRequirementKeys.GetDescription(1,4,5);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,5);
            
        }
    }
}