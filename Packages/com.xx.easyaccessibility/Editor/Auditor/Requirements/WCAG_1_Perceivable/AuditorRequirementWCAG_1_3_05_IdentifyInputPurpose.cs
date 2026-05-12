namespace EasyAccessibility
{
    /*TODO:
     * - Set up to just handle input purposes
     */
    public class AuditorRequirementWCAG_1_3_05_IdentifyInputPurpose
        : AuditorRequirementWCAG_1_1_01_NonTextContent
    {
        public AuditorRequirementWCAG_1_3_05_IdentifyInputPurpose()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 3, 5);
            description = AuditorRequirementKeys.GetDescription(1, 3, 5);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 3, 5);
        }
    }
}
