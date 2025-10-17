using UnityEngine.Video;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_08_MediaAlternativePrerecorded : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForTranscripts<VideoClip>("t:videoclip");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }




        public AuditorRequirementWCAG_1_2_08_MediaAlternativePrerecorded()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_8;
            description = AuditorRequirementKeys.Description_WCAG_1_2_8;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_8;
            status = Status.None;
        }
    }
}