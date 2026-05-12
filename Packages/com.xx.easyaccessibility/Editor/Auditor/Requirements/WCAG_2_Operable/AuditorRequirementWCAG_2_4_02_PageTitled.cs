namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_2_4_02_PageTitled : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<PageTitler>("has no PageTitler");

            if (issues.Count > 0)
                status = Status.Fail;
        }

        public AuditorRequirementWCAG_2_4_02_PageTitled()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(2, 4, 2);
            description = AuditorRequirementKeys.GetDescription(2, 4, 2);
            referenceLink = AuditorRequirementKeys.GetUrl(2, 4, 2);
        }
    }
}
