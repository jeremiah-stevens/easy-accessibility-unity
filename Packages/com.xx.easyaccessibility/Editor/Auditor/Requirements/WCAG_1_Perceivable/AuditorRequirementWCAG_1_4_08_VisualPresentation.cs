namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_4_08_VisualPresentation : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<TextSettings>("has no VisualSettingsp provider.");
        }



        public AuditorRequirementWCAG_1_4_08_VisualPresentation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_4_8;
            description = AuditorRequirementKeys.Description_WCAG_1_4_8;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_4_8;
            status = Status.None;
        }
    }
}