namespace EasyAccessibility
{
    /*TODO:
     * - Tag parts of information as specific language, likely through accessibility element
     */
    public class AuditorRequirementWCAG_3_1_02_LanguageOfParts : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_1_02_LanguageOfParts()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 1, 2);
            description = AuditorRequirementKeys.GetDescription(3, 1, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 1, 2);
        }
    }
}
