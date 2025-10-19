namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_02_CaptionsPrerecorded : AuditorRequirementWCAG_1_2_04_CaptionsLive
    {
        public AuditorRequirementWCAG_1_2_02_CaptionsPrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,2,2);
            description = AuditorRequirementKeys.GetDescription(1,2,2);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,2);
            
        }
    }
}