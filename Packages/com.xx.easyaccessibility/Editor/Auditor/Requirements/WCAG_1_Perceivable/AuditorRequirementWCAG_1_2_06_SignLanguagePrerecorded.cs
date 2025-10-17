namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_06_SignLanguagePrerecorded : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditScenes();

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }

        private void AuditScenes()
        {
            AuditForInstanceWithCamera<SignLanguageProvider>("has no Sign Language provider");
        }




        public AuditorRequirementWCAG_1_2_06_SignLanguagePrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_6;
            description = AuditorRequirementKeys.Description_WCAG_1_2_6;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_6;
            status = Status.None;
        }
    }
}