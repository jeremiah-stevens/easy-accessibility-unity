using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace EasyAccessibility
{
    public class AuditorRequirementWCAG1_2_1_AudioVideoOnlyContent : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            var assetTranscriptGuids = AssetDatabase.FindAssetGUIDs("t:assettranscriptso", new[] { "Assets" }); //TODO: find a way of finding IAssetTranscripts
            var dict = new Dictionary<UnityEngine.Object, AssetTranscriptSO>();
            foreach(var curr in assetTranscriptGuids)
            {
                var currTranscriptSO = AssetDatabase.LoadAssetByGUID<AssetTranscriptSO>(curr);
                dict.Add(currTranscriptSO.Asset, currTranscriptSO);
            }

            AuditAssetType<AudioClip>(dict, "t:audioclip");
            AuditAssetType<VideoClip>(dict, "t:videoclip");

            this.status = (issues.Count == 0) ? Status.Pass : Status.Fail;
        }

        private void AuditAssetType<T>(Dictionary<UnityEngine.Object, AssetTranscriptSO> transcripts, string searchParam) where T : UnityEngine.Object
        {
            var audioClips = AssetDatabase.FindAssetGUIDs(searchParam, new[] { "Assets" });
            foreach(var curr in audioClips)
            {
                var currAsset = AssetDatabase.LoadAssetByGUID<T>(curr);
                if(transcripts.ContainsKey(currAsset))
                {
                    //TODO: validate transcript information
                }
                else
                {
                    issues.Add(new Issue()
                    {
                        asset = currAsset,
                        issue = $"Asset '{currAsset.name}' does not have transcript information."
                    });
                }
            }
        }



        public AuditorRequirementWCAG1_2_1_AudioVideoOnlyContent()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_2_1;
            description = AuditorRequirementKeys.Description_WCAG_1_2_1;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_2_1;
            status = Status.None;
        }
    }
}