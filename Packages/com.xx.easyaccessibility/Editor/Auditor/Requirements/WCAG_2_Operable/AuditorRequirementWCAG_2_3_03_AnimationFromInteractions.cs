namespace EasyAccessibility
{
    /*TODO:
     * - Figure out how to test this
     */
    public class AuditorRequirementWCAG_2_3_03_AnimationFromInteractions : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<AnimationSettings>("has no AnimationSettings provider");

            if (issues.Count > 0) status = Status.Fail;
        }




        public AuditorRequirementWCAG_2_3_03_AnimationFromInteractions()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 3, 3);
            description = AuditorRequirementKeys.GetDescription(2, 3, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 3, 3);
        }
    }
}