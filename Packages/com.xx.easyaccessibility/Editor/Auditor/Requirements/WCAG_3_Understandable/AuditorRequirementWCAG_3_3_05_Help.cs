namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; check for input forms and ensure they have help pages in some way?
     */
    public class AuditorRequirementWCAG_3_3_05_Help : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_05_Help()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 5);
            description = AuditorRequirementKeys.GetDescription(3, 3, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 5);
        }
    }
}