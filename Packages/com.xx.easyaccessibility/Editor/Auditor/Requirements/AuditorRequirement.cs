using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace EasyAccessibility
{
    /*TODO:
     * - Update AuditForInstanceWithCamera to find all monobehaviours and then find those that are of the interface type
     * - Consider setting up as a ScriptableObject instead; find the commonalities and write up methods + SOs for each
     */
    [Serializable]
    public class AuditorRequirement
    {
        public string name;
        public string source;
        public string description;
        public string referenceLink;
        public Status status = Status.None;
        public List<Issue> issues = new();




        public virtual void Audit()
        {
            Debug.Log($"Performing audit '{this.GetType().Name}'...");

            this.status = Status.Unsure; //by default, we set it to unsure
        }

        protected void AuditForInstanceWithCamera<T>(string failMessage, Action<T> validationAction = null) where T : UnityEngine.Object
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
                    var instance = GameObject.FindFirstObjectByType<T>(FindObjectsInactive.Include);

                    if (instance == null)
                    {
                        issues.Add(new Issue()
                        {
                            asset = AssetDatabase.LoadAssetAtPath<SceneAsset>(cam.scene.path),
                            issue = $"Scene '{cam.scene.name}' {failMessage}"
                        });
                    }
                    else if(validationAction != null)
                    {
                        validationAction.Invoke(instance);
                    }
                }
            }

            EditorSceneManager.OpenScene(startingScenePath, OpenSceneMode.Single);
        }

        protected void AuditForTranscripts<T>(string searchPattern) where T : UnityEngine.Object
        {
            var assetTranscriptGuids = AssetDatabase.FindAssetGUIDs("t:assettranscriptso", new[] { "Assets" }); //TODO: find a way of finding IAssetTranscripts
            var transcripts = new Dictionary<UnityEngine.Object, AssetTranscriptSO>();
            foreach (var curr in assetTranscriptGuids)
            {
                var currTranscriptSO = AssetDatabase.LoadAssetByGUID<AssetTranscriptSO>(curr);
                transcripts.Add(currTranscriptSO.Asset, currTranscriptSO);
            }

            var assets = AssetDatabase.FindAssetGUIDs(searchPattern, new[] { "Assets" });
            foreach (var curr in assets)
            {
                var currAsset = AssetDatabase.LoadAssetByGUID<T>(curr);
                if (transcripts.ContainsKey(currAsset))
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

        public static bool UsesBuiltInRenderPipeline()
        {
            bool hasQualityLevelWithNoRenderPipeline = false;
            QualitySettings.ForEach(() =>
            {
                if(QualitySettings.renderPipeline == null)
                    hasQualityLevelWithNoRenderPipeline = true;
            });

            return GraphicsSettings.defaultRenderPipeline == null
            && hasQualityLevelWithNoRenderPipeline;
        }




        public enum Status
        {
            None = 0,
            Pass = 1,
            Unsure = 2,
            Fail = 3,
        }




        [Serializable]
        public class Issue
        {
            public UnityEngine.Object asset;
            public string issue;
        }
    }
}