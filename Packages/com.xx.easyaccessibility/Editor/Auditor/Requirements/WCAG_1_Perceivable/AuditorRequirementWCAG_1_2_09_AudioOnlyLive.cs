namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_09_AudioOnlyLive : AuditorRequirementWCAG_1_2_04_CaptionsLive
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForInstanceWithCamera<LiveCaptions>("has no LiveCaptions provider");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }




        public AuditorRequirementWCAG_1_2_09_AudioOnlyLive()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_9;
            description = AuditorRequirementKeys.Description_WCAG_1_2_9;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_9;
            status = Status.None;
        }
    }
}