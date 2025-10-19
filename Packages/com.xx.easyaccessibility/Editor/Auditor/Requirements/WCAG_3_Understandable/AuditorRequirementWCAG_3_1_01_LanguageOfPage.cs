namespace EasyAccessibility
{
    /*TODO:
     * - Should be pretty easy to hook up with the localization system
     */
    public class AuditorRequirementWCAG_3_1_01_LanguageOfPage : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<PageLanguageProvider>("has no Page Language provider");

            if (issues.Count > 0) status = Status.Fail;
        }

        public AuditorRequirementWCAG_3_1_01_LanguageOfPage()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3,1,1);
            description = AuditorRequirementKeys.GetDescription(3,1,1);
            referenceLink = AuditorRequirementKeys.GetUrl(3,1,1);
        }
    }
}