using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG1_2_4_CaptionsLive : AuditorRequirement
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




        public AuditorRequirementWCAG1_2_4_CaptionsLive()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_4;
            description = AuditorRequirementKeys.Description_WCAG_1_2_4;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_4;
            status = Status.None;
        }
    }
}