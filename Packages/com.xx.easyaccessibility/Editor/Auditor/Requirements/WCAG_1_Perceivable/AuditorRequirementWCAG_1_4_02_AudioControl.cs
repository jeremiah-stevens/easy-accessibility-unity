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

            if (issues.Count > 0) status = Status.Fail;
        }



        public AuditorRequirementWCAG_1_4_02_AudioControl()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_2;
            description = AuditorRequirementKeys.Description_WCAG_1_4_2;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_2;
            status = Status.None;
        }
    }
}