namespace EasyAccessibility
{
    /*TODO:
     * - Find a way of testing this; review all static UI for contrast?
     */
    public class AuditorRequirementWCAG_1_4_03_ContrastMinimum : AuditorRequirement
    {
        private float contrastRatio = 4.5f; //4.5:1




        public AuditorRequirementWCAG_1_4_03_ContrastMinimum()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,3);
            description = AuditorRequirementKeys.GetDescription(1,4,3);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,3);
            status = Status.None;
        }
    }
}