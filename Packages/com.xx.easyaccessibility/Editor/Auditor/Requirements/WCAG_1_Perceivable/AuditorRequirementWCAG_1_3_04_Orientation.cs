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
            name = AuditorRequirementKeys.Title_WCAG_1_3_4;
            description = AuditorRequirementKeys.Description_WCAG_1_3_4;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_3_4;
            status = Status.None;
        }
    }
}