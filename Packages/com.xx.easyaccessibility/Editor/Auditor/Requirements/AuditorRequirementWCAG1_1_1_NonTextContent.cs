using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using NUnit.Framework;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using TMPro;
using System.Text;

namespace EasyAccessibility
{
    [CreateAssetMenu(fileName = "AuditorRequirementWCAG1_1_Title", menuName = "Scriptable Objects/AuditorRequirementWCAG1_1_Title")]
    public class AuditorRequirementWCAG1_1_1_NonTextContent : AuditorRequirement
    {
        public override void Audit()
        {
            /*TODO:
             * * Check every prefab for the given elements
             * * Check every scene for the given elements
             */

            issues.Clear();

            var prefabs = AssetDatabase.FindAssetGUIDs("t:prefab", new[] {"Assets"});
            foreach(var prefab in prefabs)
            {
                var obj = AssetDatabase.LoadAssetByGUID<GameObject>(prefab);

                AuditComponents<UnityEngine.UI.Image>(obj);
                AuditComponents<RawImage>(obj);
                AuditComponents<UnityEngine.UI.Toggle>(obj);
                AuditComponents<UnityEngine.UI.Slider>(obj);
                AuditComponents<Scrollbar>(obj);
                AuditComponents<UnityEngine.UI.Button>(obj);
                AuditComponents<Dropdown>(obj);
                AuditComponents<InputField>(obj);
                AuditComponents<Text>(obj);
                AuditComponents<TMP_InputField>(obj);
                AuditComponents<TMP_Text>(obj);
                AuditComponents<TMP_Dropdown>(obj);
            }

            this.status = (issues.Count == 0) ? Status.Unsure : Status.Fail;
        }

        private void AuditComponents<T>(GameObject go) where T : UnityEngine.Component
        {
            var components = go.GetComponentsInChildren<T>(includeInactive: true);

            StringBuilder sb = new();

            foreach(var curr in components)
            {
                var accessibilityElement = curr.transform.GetComponent<AccessibilityElement>();
                if (accessibilityElement == null)
                {
                    sb.Clear();
                    sb.Append(curr.name);
                    var parent = curr.transform.parent;

                    while(parent != null)
                    {
                        sb.Insert(0, parent.name+"/");
                        parent = parent.parent;
                    }

                    issues.Add(new Issue()
                    {
                        asset = go,
                        issue = $"Object '{sb.ToString()}' is missing an accessibility element."
                    });
                }
                else
                {
                    //TODO: validate information is properly set on the element
                }
            }
        }




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