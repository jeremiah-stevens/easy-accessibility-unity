namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_3_02_ThreeFlashes : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_3_02_ThreeFlashes()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 3, 2);
            description = AuditorRequirementKeys.GetDescription(2, 3, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 3, 2);
        }
    }
}