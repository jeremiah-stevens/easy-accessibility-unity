using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_04_CaptionsLive : AuditorRequirement
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
            AuditForInstanceWithCamera<LiveCaptions>("has no LiveCaptions provider");
        }




        public AuditorRequirementWCAG_1_2_04_CaptionsLive()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,2,4);
            description = AuditorRequirementKeys.GetDescription(1,2,4);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,4);
            
        }
    }
}