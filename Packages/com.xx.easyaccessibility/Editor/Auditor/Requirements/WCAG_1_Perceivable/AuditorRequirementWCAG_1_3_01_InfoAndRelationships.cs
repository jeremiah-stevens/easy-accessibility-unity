namespace EasyAccessibility
{
    /*TODO:
     * - set up conditional checking for ARIA information
     */
    public class AuditorRequirementWCAG_1_3_01_InfoAndRelationships : AuditorRequirementWCAG_1_1_01_NonTextContent
    {
        public AuditorRequirementWCAG_1_3_01_InfoAndRelationships()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,3,1);
            description = AuditorRequirementKeys.GetDescription(1,3,1);
            referenceLink = AuditorRequirementKeys.GetUrl(1,3,1);
            
        }
    }
}