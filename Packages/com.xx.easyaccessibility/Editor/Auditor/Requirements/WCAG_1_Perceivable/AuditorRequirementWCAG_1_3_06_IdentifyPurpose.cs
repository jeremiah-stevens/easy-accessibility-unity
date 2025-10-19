namespace EasyAccessibility
{
    /*TODO:
     * - Set up to sanity check information
     */
    public class AuditorRequirementWCAG_1_3_06_IdentifyPurpose : AuditorRequirementWCAG_1_1_01_NonTextContent
    {
        public AuditorRequirementWCAG_1_3_06_IdentifyPurpose()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,3,6);
            description = AuditorRequirementKeys.GetDescription(1,3,6);
            referenceLink = AuditorRequirementKeys.GetUrl(1,3,6);
            
        }
    }
}