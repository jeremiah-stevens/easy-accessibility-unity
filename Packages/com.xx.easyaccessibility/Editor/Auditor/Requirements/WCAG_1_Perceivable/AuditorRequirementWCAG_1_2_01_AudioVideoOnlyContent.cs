using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG_1_2_01_AudioVideoOnlyContent : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditForTranscripts<AudioClip>("t:audioclip");
            AuditForTranscripts<VideoClip>("t:videoclip");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }




        public AuditorRequirementWCAG_1_2_01_AudioVideoOnlyContent()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1, 2, 1);
            description = AuditorRequirementKeys.GetDescription(1, 2, 1);
            referenceLink = AuditorRequirementKeys.GetUrl(1,2,1);
            
        }
    }
}