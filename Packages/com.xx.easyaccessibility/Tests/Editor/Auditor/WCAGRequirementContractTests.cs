using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EasyAccessibility.Tests.Auditor
{
    public class WCAGRequirementContractTests
    {
        static IEnumerable<TestCaseData> AllRequirementTypes() =>
            typeof(AuditorRequirement)
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(AuditorRequirement)) && !t.IsAbstract)
                .Select(t => new TestCaseData(t).SetName(t.Name));

        static AuditorRequirement Create(Type type) =>
            (AuditorRequirement)Activator.CreateInstance(type);

        [TestCaseSource(nameof(AllRequirementTypes))]
        public void Ctor_PopulatesName(Type type)
        {
            var req = Create(type);
            Assert.IsNotNull(req.name);
            Assert.IsNotEmpty(req.name);
        }

        [TestCaseSource(nameof(AllRequirementTypes))]
        public void Ctor_PopulatesDescription(Type type)
        {
            var req = Create(type);
            Assert.IsNotNull(req.description);
            Assert.IsNotEmpty(req.description);
        }

        [TestCaseSource(nameof(AllRequirementTypes))]
        public void Ctor_PopulatesReferenceLink(Type type)
        {
            var req = Create(type);
            StringAssert.StartsWith("https://", req.referenceLink);
        }

        [TestCaseSource(nameof(AllRequirementTypes))]
        public void Ctor_PopulatesSource(Type type)
        {
            var req = Create(type);
            Assert.IsNotNull(req.source);
            Assert.IsNotEmpty(req.source);
        }

        [TestCaseSource(nameof(AllRequirementTypes))]
        public void Ctor_DefaultStatus_IsNone(Type type)
        {
            var req = Create(type);
            Assert.AreEqual(AuditorRequirement.Status.None, req.status);
        }
    }
}
