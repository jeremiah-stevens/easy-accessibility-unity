using EasyAccessibility.DescriptiveMedia;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_09_AudioOnlyLive
        : AuditorRequirementWCAG_1_2_04_CaptionsLive
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForInstanceWithCamera<DescriptiveMediaSource>(
                "has no DescriptiveMediaSource, add one to any GameObject with an AudioSource"
            );

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }

        public AuditorRequirementWCAG_1_2_09_AudioOnlyLive()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 2, 9);
            description = AuditorRequirementKeys.GetDescription(1, 2, 9);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 2, 9);
        }
    }
}
