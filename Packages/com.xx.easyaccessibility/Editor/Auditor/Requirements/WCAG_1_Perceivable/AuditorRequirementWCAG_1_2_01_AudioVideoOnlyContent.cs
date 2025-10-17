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
            name = AuditorRequirementKeys.Title_WCAG_1_2_1;
            description = AuditorRequirementKeys.Description_WCAG_1_2_1;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_1;
            status = Status.None;
        }
    }
}