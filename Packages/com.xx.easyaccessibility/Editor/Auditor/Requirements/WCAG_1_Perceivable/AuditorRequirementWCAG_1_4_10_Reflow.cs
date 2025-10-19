using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace EasyAccessibility
{
    /*TODO:
     * - Set up some way of evaluating that elements fit on the page for the given width (ex: right-anchored elements don't override left-anchored ones)
     */
    public class AuditorRequirementWCAG_1_4_10_Reflow : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            AuditPrefabs();
            AuditScenes();

            if (issues.Count > 0) status = Status.Fail;
        }

        private void AuditPrefabs()
        {
            var prefabs = AssetDatabase.FindAssetGUIDs("t:prefab", new[] { "Assets" });
            foreach (var prefab in prefabs)
            {
                var obj = AssetDatabase.LoadAssetByGUID<GameObject>(prefab);
                AuditObject(obj);
            }
        }

        private void AuditScenes()
        {
            var startingScenePath = EditorSceneManager.GetActiveScene().path;

            var scenes = AssetDatabase.FindAssetGUIDs("t:scene", new[] { "Assets" });
            foreach (var scene in scenes)
            {
                var path = AssetDatabase.GUIDToAssetPath(scene);
                var currScene = EditorSceneManager.OpenScene(path, OpenSceneMode.Single);

                var objs = GameObject.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var obj in objs)
                {
                    AuditObject(obj, markScene: true);
                }
            }

            EditorSceneManager.OpenScene(startingScenePath, OpenSceneMode.Single);
        }

        private void AuditObject(GameObject gameObject, bool markScene = false)
        {
            AuditComponents(gameObject, markScene);
        }

        private void AuditComponents(GameObject gameObject, bool markScene = false)
        {
            var components = gameObject.GetComponentsInChildren<ScrollRect>(includeInactive: true);

            StringBuilder sb = new();

            foreach(var curr in components)
            {
                if(curr.horizontal && curr.vertical)
                {
                    sb.Clear();
                    sb.Append(curr.name);
                    var parent = curr.transform.parent;

                    while (parent != null)
                    {
                        sb.Insert(0, parent.name + "/");
                        parent = parent.parent;
                    }

                    issues.Add(new Issue()
                    {
                        asset = (markScene) ? AssetDatabase.LoadAssetAtPath<SceneAsset>(gameObject.scene.path) : gameObject,
                        issue = $"Object '{sb.ToString()}' must be scrollable along one axis."
                    });
                }
            }
        }




        public AuditorRequirementWCAG_1_4_10_Reflow()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.GetTitle(1,4,10);
            description = AuditorRequirementKeys.GetDescription(1,4,10);
            referenceLink = AuditorRequirementKeys.GetUrl(1,4,10);
            status = Status.None;
        }
    }
}