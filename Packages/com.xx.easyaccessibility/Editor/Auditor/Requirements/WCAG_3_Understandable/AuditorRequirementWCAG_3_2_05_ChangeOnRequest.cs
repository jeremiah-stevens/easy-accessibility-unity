namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; consider looking for automatic animations/movements of objects  (ex: rotating pages in a collection)
     */
    public class AuditorRequirementWCAG_3_2_05_ChangeOnRequest : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_2_05_ChangeOnRequest()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 2, 5);
            description = AuditorRequirementKeys.GetDescription(3, 2, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 2, 5);
        }
    }
}