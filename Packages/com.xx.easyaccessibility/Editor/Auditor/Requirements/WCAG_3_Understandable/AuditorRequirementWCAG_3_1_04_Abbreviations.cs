namespace EasyAccessibility
{
    /*TODO:
     * - Audit for abbreviations (look for words that are spoken regularly in dialog system but not present in keyword explainer)
     * - Consider making a subclass of AuditorRequirementWCAG_3_1_03_UnusualWords
     */
    public class AuditorRequirementWCAG_3_1_04_Abbreviations : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditForInstanceWithCamera<KeywordExplainer>("has no Keyword Explainer");
        }

        public AuditorRequirementWCAG_3_1_04_Abbreviations()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(3, 1, 4);
            description = AuditorRequirementKeys.GetDescription(3, 1, 4);
            referenceLink = AuditorRequirementKeys.GetUrl(3, 1, 4);
        }
    }
}
