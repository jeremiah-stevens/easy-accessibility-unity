namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_05_AudioDescriptionPrerecorded : AuditorRequirementWCAG_1_2_01_AudioVideoOnlyContent
    {
        public AuditorRequirementWCAG_1_2_05_AudioDescriptionPrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,2,5);
            description = AuditorRequirementKeys.GetDescription(1,2,5);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,5);
            
        }
    }
}