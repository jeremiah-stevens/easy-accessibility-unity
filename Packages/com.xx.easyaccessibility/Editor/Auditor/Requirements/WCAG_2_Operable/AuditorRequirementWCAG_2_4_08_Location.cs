namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; likely using a breadcrumbs implementation
     */
    public class AuditorRequirementWCAG_2_4_08_Location : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_08_Location()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 8);
            description = AuditorRequirementKeys.GetDescription(2, 4, 8);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 8);
        }
    }
}
