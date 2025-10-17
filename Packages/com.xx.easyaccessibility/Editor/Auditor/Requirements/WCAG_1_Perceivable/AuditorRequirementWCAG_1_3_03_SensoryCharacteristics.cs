namespace EasyAccessibility
{
    /*TODO:
     * - How do we test for this? Possibly check all scripts for mentions of keywords (ex: red) and make sure the dev checks them all?
     */ 
    public class AuditorRequirementWCAG_1_3_03_SensoryCharacteristics : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_3_03_SensoryCharacteristics()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_3_3;
            description = AuditorRequirementKeys.Description_WCAG_1_3_3;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_3_3;
            status = Status.None;
        }
    }
}