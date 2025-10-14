using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using NUnit.Framework;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using TMPro;
using System.Text;
using UnityEditor.SceneManagement;

namespace EasyAccessibility
{
    /*TODO:
     * - Consider allowing settings for checking assets more granularly (ex: also check packages, only check those that will be included in build, etc.)
     * - Duplicate checks (referencing same object and same path)
     */
    public class AuditorRequirementWCAG1_1_1_NonTextContent : AuditorRequirement
    {
        public override void Audit()
        {
            base.Audit();

            issues.Clear();

            AuditPrefabs();

            AuditUIToolkit();

            AuditScenes();

            this.status = (issues.Count == 0) ? Status.Unsure : Status.Fail;
        }




        #region Audit GameObjects (Prefabs, Scenes)

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
            AuditComponents<UnityEngine.UI.Image>(gameObject, markScene);
            AuditComponents<RawImage>(gameObject, markScene);
            AuditComponents<UnityEngine.UI.Toggle>(gameObject, markScene);
            AuditComponents<UnityEngine.UI.Slider>(gameObject, markScene);
            AuditComponents<Scrollbar>(gameObject, markScene);
            AuditComponents<UnityEngine.UI.Button>(gameObject, markScene);
            AuditComponents<Dropdown>(gameObject, markScene);
            AuditComponents<InputField>(gameObject, markScene);
            AuditComponents<Text>(gameObject, markScene);
            AuditComponents<TMP_InputField>(gameObject, markScene);
            AuditComponents<TMP_Text>(gameObject, markScene);
            AuditComponents<TMP_Dropdown>(gameObject, markScene);
        }

        private void AuditComponents<T>(GameObject gameObject, bool markScene = false) where T : UnityEngine.Component
        {
            var components = gameObject.GetComponentsInChildren<T>(includeInactive: true);

            StringBuilder sb = new();

            foreach (var curr in components)
            {
                var accessibilityElement = curr.transform.GetComponent<AccessibilityElement>();
                if (accessibilityElement == null)
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
                        issue = $"Object '{sb.ToString()}' is missing an accessibility element."
                    });
                }
                else
                {
                    //TODO: validate information is properly set on the element
                }
            }
        }

        #endregion



        #region Audit UIToolkit

        private void AuditUIToolkit()
        {
            var visualTreeAssets = AssetDatabase.FindAssetGUIDs("t:visualtreeasset", new[] { "Assets" });
            foreach (var asset in visualTreeAssets)
            {
                var obj = AssetDatabase.LoadAssetByGUID<VisualTreeAsset>(asset);

                foreach(var curr in obj.Instantiate().hierarchy.Children())
                {
                    AuditVisualElementsRecursive(curr, obj, "");
                }
            }
        }

        private void AuditVisualElementsRecursive(VisualElement rootElement, VisualTreeAsset asset, string subpath)
        {
            AuditVisualElement(rootElement, asset, subpath + $"/{rootElement.name}");

            foreach(var curr in rootElement.Children())
            {
                AuditVisualElementsRecursive(curr, asset, subpath + $"/{rootElement.name}");
            }
        }

        private void AuditVisualElement(VisualElement visualElement, VisualTreeAsset asset, string path)
        {
            issues.Add(new Issue() //TODO: should find some way of checking if was actually assigned to accessibility node, leaving as false positive as starting point
            {
                asset = asset,
                issue = $"Visual Element '{path}' is not assigned to Accessibility Hierarchy"
            });
        }

        #endregion




        public AuditorRequirementWCAG1_1_1_NonTextContent()
        {
            source = AuditorRequirementKeys.Source_WCAG;
            name = AuditorRequirementKeys.Title_WCAG_1_1_1;
            description = AuditorRequirementKeys.Description_WCAG_1_1_1;
            referenceLink = AuditorRequirementKeys.URL_WCAG_1_1_1;
            status = Status.None;
        }
    }
}