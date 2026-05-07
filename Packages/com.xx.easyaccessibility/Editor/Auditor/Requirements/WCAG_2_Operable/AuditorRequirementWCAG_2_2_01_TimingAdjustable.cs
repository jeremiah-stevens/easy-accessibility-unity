namespace EasyAccessibility
{
    /*TODO:
     * - Finish implementing
     * - Find way of evaluating for functionality (ex: QTEs, is it possible to offer this for real-time content?)
     */
    public class AuditorRequirementWCAG_2_2_01_TimingAdjustable : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<TimingSettings>("has no TimingSettings provider.");
        }




        public AuditorRequirementWCAG_2_2_01_TimingAdjustable()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 2, 1);
            description = AuditorRequirementKeys.GetDescription(2, 2, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 2, 1);
        }
    }
}