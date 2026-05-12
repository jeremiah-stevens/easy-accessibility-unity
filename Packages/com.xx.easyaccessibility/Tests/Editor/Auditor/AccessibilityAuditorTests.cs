using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace EasyAccessibility.Tests.Auditor
{
    public class AccessibilityAuditorTests
    {
        const string k_TestAssetPath = "Assets/Editor/Accessibility/TestReport.asset";
        bool m_createdTestAsset;

        [SetUp]
        public void SetUp()
        {
            m_createdTestAsset = false;
            var existing = AssetDatabase.FindAssets("t:auditorrequirementsso");
            foreach (var guid in existing)
                AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));
        }

        [TearDown]
        public void TearDown()
        {
            if (m_createdTestAsset)
                AssetDatabase.DeleteAsset(k_TestAssetPath);
        }

        #region HasExistingReport

        [Test]
        public void HasExistingReport_WhenNoReport_ReturnsFalse()
        {
            Assert.IsFalse(AccessibilityAuditor.HasExistingReport());
        }

        [Test]
        public void HasExistingReport_WhenReportExists_ReturnsTrue()
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(k_TestAssetPath));
            var so = ScriptableObject.CreateInstance<AuditorRequirementsSO>();
            AssetDatabase.CreateAsset(so, k_TestAssetPath);
            AssetDatabase.SaveAssets();
            m_createdTestAsset = true;

            Assert.IsTrue(AccessibilityAuditor.HasExistingReport());
        }

        #endregion
    }
}