using NUnit.Framework;

namespace EasyAccessibility.Tests.Auditor
{
    public class AuditorRequirementKeysTests
    {
        #region GetTitle

        [Test]
        public void GetTitle_ContainsColonSeparator()
        {
            var title = AuditorRequirementKeys.GetTitle(1, 1, 1);
            StringAssert.Contains(": ", title);
        }

        [Test]
        public void GetTitle_DifferentCriteria_ReturnDifferentValues()
        {
            var t1 = AuditorRequirementKeys.GetTitle(1, 1, 1);
            var t2 = AuditorRequirementKeys.GetTitle(1, 2, 1);
            Assert.AreNotEqual(t1, t2);
        }

        #endregion

        #region GetDescription

        [Test]
        public void GetDescription_ReturnsNonEmptyString()
        {
            var desc = AuditorRequirementKeys.GetDescription(1, 1, 1);
            Assert.IsNotNull(desc);
            Assert.IsNotEmpty(desc);
        }

        [Test]
        public void GetDescription_DifferentCriteria_ReturnDifferentValues()
        {
            var d1 = AuditorRequirementKeys.GetDescription(1, 1, 1);
            var d2 = AuditorRequirementKeys.GetDescription(1, 2, 1);
            Assert.AreNotEqual(d1, d2);
        }

        #endregion

        #region GetUrl

        [Test]
        public void GetUrl_StartsWithWCAGBaseUrl()
        {
            var url = AuditorRequirementKeys.GetUrl(1, 1, 1);
            StringAssert.StartsWith("https://www.w3.org/WAI/WCAG22/quickref/#", url);
        }

        [Test]
        public void GetUrl_DifferentCriteria_ReturnDifferentUrls()
        {
            var u1 = AuditorRequirementKeys.GetUrl(1, 1, 1);
            var u2 = AuditorRequirementKeys.GetUrl(1, 2, 1);
            Assert.AreNotEqual(u1, u2);
        }

        #endregion
    }
}
