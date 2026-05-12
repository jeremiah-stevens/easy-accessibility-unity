using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAccessibility
{
    /*TODO:
     * - Loading bars for progress
     */
    public static class AccessibilityAuditor
    {
        public static void Audit()
        {
            var baseType = typeof(AuditorRequirement);
            var assembly = typeof(AuditorRequirement).Assembly;
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType));

            AccessibilityReport report = new();
            int issueCount = 0;

            foreach (var type in types)
            {
                var instance = (AuditorRequirement)Activator.CreateInstance(type);
                instance.Audit();
                report.requirements.Add(instance);
                issueCount += instance.issues.Count;
            }

            Debug.Log($"Finished! Found '{issueCount}' issues");

            string path = "Assets/Editor/Accessibility/AccessibilityReport.asset";
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            AuditorRequirementsSO auditorRequirements =
                ScriptableObject.CreateInstance<AuditorRequirementsSO>();
            auditorRequirements.report = report;
            AssetDatabase.CreateAsset(auditorRequirements, path);
            AssetDatabase.SaveAssets();
        }

        public static bool HasExistingReport()
        {
            var assets = AssetDatabase.FindAssets("t:auditorrequirementsso");
            return assets.Length > 0;
        }
    }
}
