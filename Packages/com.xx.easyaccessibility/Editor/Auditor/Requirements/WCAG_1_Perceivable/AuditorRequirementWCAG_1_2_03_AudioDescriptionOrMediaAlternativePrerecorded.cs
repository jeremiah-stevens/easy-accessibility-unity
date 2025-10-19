namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_03_AudioDescriptionOrMediaAlternativePrerecorded : AuditorRequirementWCAG_1_2_01_AudioVideoOnlyContent
    {
        public AuditorRequirementWCAG_1_2_03_AudioDescriptionOrMediaAlternativePrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,2,3);
            description = AuditorRequirementKeys.GetDescription(1,2,3);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,3);
            
        }
    }
}