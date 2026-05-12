namespace EasyAccessibility
{
    /*TODO:
     * - Find way of evaluating this automatically; check for key words in localization?
     */
    public class AuditorRequirementWCAG_1_4_02_AudioControl : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<AudioSettings>("has no AudioControls provider.");

            if (issues.Count > 0)
                status = Status.Fail;
        }

        public AuditorRequirementWCAG_1_4_02_AudioControl()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 4, 2);
            description = AuditorRequirementKeys.GetDescription(1, 4, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 4, 2);
        }
    }
}
