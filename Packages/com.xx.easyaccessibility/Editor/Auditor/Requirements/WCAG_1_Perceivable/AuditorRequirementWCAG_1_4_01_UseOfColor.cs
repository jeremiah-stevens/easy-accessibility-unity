namespace EasyAccessibility
{
    /*TODO:
     * - Find way of evaluating this automatically; check for key words in localization?
     */
    public class AuditorRequirementWCAG_1_4_01_UseOfColor : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_01_UseOfColor()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,1);
            description = AuditorRequirementKeys.GetDescription(1,4,1);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,1);
            
        }
    }
}