using TMPro;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_4_09_ImagesOfTextNoException : AuditorRequirementWCAG_1_4_05_ImagesOfText
    {
        public AuditorRequirementWCAG_1_4_09_ImagesOfTextNoException()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,9);
            description = AuditorRequirementKeys.GetDescription(1,4,9);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,9);
            status = Status.None;
        }
    }
}