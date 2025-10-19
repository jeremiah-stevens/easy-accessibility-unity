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
            name = AuditorRequirementKeys.GetTitle(1,2,8);
            description = AuditorRequirementKeys.GetDescription(1,2,8);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,8);
            
        }
    }
}