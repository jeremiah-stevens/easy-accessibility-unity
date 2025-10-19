namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; likely similar to WCAG 1.1.1
     */
    public class AuditorRequirementWCAG_4_1_02_NameRoleValue : AuditorRequirement
    {
        public AuditorRequirementWCAG_4_1_02_NameRoleValue()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(4,1,2);
            description = AuditorRequirementKeys.GetDescription(4,1,2);
            referenceLink = AuditorRequirementKeys.GetUrl(4,1,2);
        }
    }
}