namespace EasyAccessibility
{
    /*TODO:
     * - Come up with a way of testing orientation (showing all static elements in both orientations and make sure visible or put it on the dev?)
     */
    public class AuditorRequirementWCAG_1_3_04_Orientation : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_3_04_Orientation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 3, 4);
            description = AuditorRequirementKeys.GetDescription(1, 3, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 3, 4);
        }
    }
}
