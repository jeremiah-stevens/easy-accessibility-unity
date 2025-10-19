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
            name = AuditorRequirementKeys.GetTitle(1,3,3);
            description = AuditorRequirementKeys.GetDescription(1,3,3);
            referenceLink = AuditorRequirementKeys.GetUrl(1,3,3);
            status = Status.None;
        }
    }
}