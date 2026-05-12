namespace EasyAccessibility
{
    /*TODO:
     * - Check all UI for label field values; accessibility element?
     */
    public class AuditorRequirementWCAG_2_5_03_LabelInName : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_03_LabelInName()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 3);
            description = AuditorRequirementKeys.GetDescription(2, 5, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 3);
        }
    }
}
