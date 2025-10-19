namespace EasyAccessibility
{
    /*TODO:
     * - Audit for keywords (look for words that are spoken regularly in dialog system but not present in keyword explainer)
     */
    public class AuditorRequirementWCAG_3_1_03_UnusualWords : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<KeywordExplainer>("has no Keyword Explainer");
        }

        public AuditorRequirementWCAG_3_1_03_UnusualWords()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 1, 3);
            description = AuditorRequirementKeys.GetDescription(3, 1, 3);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 1, 3);
        }
    }
}