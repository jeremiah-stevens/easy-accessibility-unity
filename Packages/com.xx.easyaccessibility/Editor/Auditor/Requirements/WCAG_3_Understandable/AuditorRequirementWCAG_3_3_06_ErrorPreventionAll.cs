namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_3_3_06_ErrorPreventionAll
        : AuditorRequirementWCAG_3_3_04_ErrorPrevention
    {
        public AuditorRequirementWCAG_3_3_06_ErrorPreventionAll()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 6);
            description = AuditorRequirementKeys.GetDescription(3, 3, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 6);
        }
    }
}
