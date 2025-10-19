namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; just look for keywords/common nouns and provide pronunciation advice? Text or audio format?
     */
    public class AuditorRequirementWCAG_3_1_06_Pronunciation : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_1_06_Pronunciation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 1, 6);
            description = AuditorRequirementKeys.GetDescription(3, 1, 6);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 1, 6);
        }
    }
}