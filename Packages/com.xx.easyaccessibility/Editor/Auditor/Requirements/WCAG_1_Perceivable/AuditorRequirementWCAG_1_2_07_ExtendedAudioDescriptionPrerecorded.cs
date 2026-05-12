namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_07_ExtendedAudioDescriptionPrerecorded
        : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForInstanceWithCamera<AudioGapProvider>("has no AudioGap provider.");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }

        public AuditorRequirementWCAG_1_2_07_ExtendedAudioDescriptionPrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 2, 7);
            description = AuditorRequirementKeys.GetDescription(1, 2, 7);
            referenceLink = AuditorRequirementKeys.GetUrl(1, 2, 7);
        }
    }
}
