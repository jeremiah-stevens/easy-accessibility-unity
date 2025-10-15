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
            var startingScenePath = EditorSceneManager.GetActiveScene().path;

            var scenes = AssetDatabase.FindAssetGUIDs("t:scene", new[] { "Assets" });
            foreach (var scene in scenes)
            {
                var path = AssetDatabase.GUIDToAssetPath(scene);
                var currScene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                var cam = GameObject.FindAnyObjectByType<Camera>(FindObjectsInactive.Include);
                if (cam)
                {
                    var caption = GameObject.FindFirstObjectByType<LiveCaptions>(FindObjectsInactive.Include);

                    if (caption == null)
                    {
                        issues.Add(new Issue()
                        {
                            asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(cam.scene.path),
                            issue = $"Scene '{cam.scene.name}' has no LiveCaptions provider."
                        });
                    }
                }
            }

            EditorSceneManager.OpenScene(startingScenePath, OpenSceneMode.Single);
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