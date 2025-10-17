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
            name = AuditorRequirementKeys.Title_WCAG_1_4_3;
            description = AuditorRequirementKeys.Description_WCAG_1_4_3;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_3;
            status = Status.None;
        }
    }
}