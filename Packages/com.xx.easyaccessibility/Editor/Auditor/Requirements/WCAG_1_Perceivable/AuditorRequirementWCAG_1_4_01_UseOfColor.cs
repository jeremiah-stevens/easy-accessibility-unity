using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace EasyAccessibility
{
    /*TODO:
     * - Find way of evaluating this automatically; check for mentions of color in localization?
     * - Add support for HDRP and Built-In checks
     */
    public class AuditorRequirementWCAG_1_4_01_UseOfColor : AuditorRequirement
    {
        public AuditorRequirementWCAG_1_4_01_UseOfColor()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,1);
            description = AuditorRequirementKeys.GetDescription(1,4,1);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,1);
        }




        public override void Audit()
        {
            base.Audit();

            //checks for colorblind correction
            VerifyAllUniversalRendererDataHasColorblindnessSetting();
            VerifyAllScenesHaveACameraWithColorblindCorrectionBuiltIn();
            VerifyAllScenesHaveHDRPVolumeWithColorblindCorrectionHDRP();
        }

        private void VerifyAllUniversalRendererDataHasColorblindnessSetting()
        {
#if EA_URP
            var guids = AssetDatabase.FindAssets("t:UniversalRendererData");

            foreach (var guid in guids)
            {
                var curr = AssetDatabase.LoadAssetByGUID<UniversalRendererData>(new GUID(guid));

                if(curr != null)
                {
                    var index = curr.rendererFeatures.FindIndex((feature) => feature is ColorblindnessRendererFeature);

                    if(index == -1)
                    {
                        issues.Add(new Issue()
                        {
                            asset = curr,
                            issue = $"UniversalRendererData '{curr}' does not include a colorblind correction feature. Consider adding one to offer colorblind correction support."
                        });
                    }
                }
            }
#endif
        }

        private void VerifyAllScenesHaveACameraWithColorblindCorrectionBuiltIn()
        {
            if(!UsesBuiltInRenderPipeline()) return; //don't check for this if there is no built-in render pipeline usage

            AuditForInstanceWithCamera<ColorblindCorrectionBuiltIn>("No colorblind correction feature on camera");
        }

        private void VerifyAllScenesHaveHDRPVolumeWithColorblindCorrectionHDRP()
        {
#if EA_HDRP
            AuditForInstanceWithCamera<Volume>("No volume in the given scene.", (volume) =>
            {
                var index = volume.profile.components.FindIndex((i) => i is ColorblindCorrectionHDRP);

                if(index == -1)
                {
                    issues.Add(new Issue()
                    {
                        asset = volume.profile,
                        issue = $"Volume Profile '{volume.profile}' does not include a colorblind correction feature. Consider adding one to offer colorblind correction support."
                    });
                }
            });
#endif
        }
    }
}