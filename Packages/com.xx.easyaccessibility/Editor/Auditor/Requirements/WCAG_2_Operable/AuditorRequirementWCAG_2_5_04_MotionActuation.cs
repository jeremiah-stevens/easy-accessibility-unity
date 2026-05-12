namespace EasyAccessibility
{
    /*TODO:
     * - Check for gyro/other input settings and allow/recommend for others (ex: swiping to move camera)
     */
    public class AuditorRequirementWCAG_2_5_04_MotionActuation : AuditorRequirement
    {
        public AuditorRequirementWCAG_2_5_04_MotionActuation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 5, 4);
            description = AuditorRequirementKeys.GetDescription(2, 5, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 5, 4);
        }
    }
}
