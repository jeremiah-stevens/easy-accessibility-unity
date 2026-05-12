namespace EasyAccessibility
{
    /*TODO:
     * - Find a way to test this; likely adding in padding to items covered by scroll or by preventing stickied elements
     */
    public class AuditorRequirementWCAG_2_4_11_FocusNotObscuredMinimum : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_4_11_FocusNotObscuredMinimum()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 11);
            description = AuditorRequirementKeys.GetDescription(2, 4, 11);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 11);
        }
    }
}
