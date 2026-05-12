using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace EasyAccessibility.Tests.Auditor
{
    public class AuditorRequirementTests
    {
        AuditorRequirement m_req;

        [SetUp]
        public void SetUp()
        {
            m_req = new AuditorRequirement();
        }

        #region Defaults

        [Test]
        public void DefaultStatus_IsNone()
        {
            Assert.AreEqual(AuditorRequirement.Status.None, m_req.status);
        }

        [Test]
        public void DefaultIssues_IsEmpty()
        {
            Assert.IsNotNull(m_req.issues);
            Assert.AreEqual(0, m_req.issues.Count);
        }

        #endregion

        #region Audit

        [Test]
        public void Audit_SetsStatusToUnsure()
        {
            m_req.Audit();
            Assert.AreEqual(AuditorRequirement.Status.Unsure, m_req.status);
        }

        [Test]
        public void Audit_DoesNotAddIssues()
        {
            m_req.Audit();
            Assert.AreEqual(0, m_req.issues.Count);
        }

        [Test]
        public void Audit_LogsTypeName()
        {
            LogAssert.Expect(LogType.Log, "Performing audit 'AuditorRequirement'...");
            m_req.Audit();
        }

        #endregion

        #region UsesBuiltInRenderPipeline

        [Test]
        public void UsesBuiltInRenderPipeline_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => AuditorRequirement.UsesBuiltInRenderPipeline());
        }

        #endregion

        #region Status enum

        [Test]
        public void StatusEnum_OrderedCorrectly()
        {
            Assert.AreEqual(0, (int)AuditorRequirement.Status.None);
            Assert.AreEqual(1, (int)AuditorRequirement.Status.Pass);
            Assert.AreEqual(2, (int)AuditorRequirement.Status.Unsure);
            Assert.AreEqual(3, (int)AuditorRequirement.Status.Fail);
        }

        #endregion
    }
}