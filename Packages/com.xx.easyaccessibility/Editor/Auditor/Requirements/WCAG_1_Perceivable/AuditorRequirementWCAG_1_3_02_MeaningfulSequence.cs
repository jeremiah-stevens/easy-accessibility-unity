namespace EasyAccessibility
{
    /*TODO:
     * - Relevant or just put on the user to handle?
     */
    public class AuditorRequirementWCAG_1_3_02_MeaningfulSequence : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_3_02_MeaningfulSequence()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 3, 2);
            description = AuditorRequirementKeys.GetDescription(1, 3, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 3, 2);
        }
    }
}
