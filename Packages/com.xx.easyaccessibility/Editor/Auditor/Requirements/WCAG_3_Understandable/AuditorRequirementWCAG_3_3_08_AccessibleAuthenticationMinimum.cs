namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to evaluate; best would probably be just offering password manager support, but how to do in Unity?
     */
    public class AuditorRequirementWCAG_3_3_08_AccessibleAuthenticationMinimum : AuditorRequirement
    {
        public AuditorRequirementWCAG_3_3_08_AccessibleAuthenticationMinimum()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 3, 8);
            description = AuditorRequirementKeys.GetDescription(3, 3, 8);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 3, 8);
        }
    }
}