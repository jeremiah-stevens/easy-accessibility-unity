namespace EasyAccessibility
{
    public class AuditorRequirementWCAG1_2_7_ExtendedAudioDescriptionPrerecorded : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForInstanceWithCamera<AudioGapProvider>("has no AudioGap provider.");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }




        public AuditorRequirementWCAG1_2_7_ExtendedAudioDescriptionPrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_7;
            description = AuditorRequirementKeys.Description_WCAG_1_2_7;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_7;
            status = Status.None;
        }
    }
}