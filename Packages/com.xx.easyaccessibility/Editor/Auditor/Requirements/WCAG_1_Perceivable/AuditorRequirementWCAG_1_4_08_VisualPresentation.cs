namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_4_08_VisualPresentation : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<TextSettings>("has no VisualSettings provider.");
        }



        public AuditorRequirementWCAG_1_4_08_VisualPresentation()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,8);
            description = AuditorRequirementKeys.GetDescription(1,4,8);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,8);
            
        }
    }
}