using NUnit.Framework;

namespace EasyAccessibility.Tests.RebindableKeys
{
    public class ExcludedControlTests
    {
        #region None

        [Test]
        public void None_IsZero()
        {
            Assert.AreEqual(0, (int)ExcludedControl.None);
        }

        #endregion

        #region Flag values

        [Test]
        [TestCase(ExcludedControl.MousePosition, 1)]
        [TestCase(ExcludedControl.MouseDelta, 2)]
        [TestCase(ExcludedControl.MouseScroll, 4)]
        [TestCase(ExcludedControl.PointerPosition, 8)]
        [TestCase(ExcludedControl.TouchPosition, 16)]
        public void FlagValues_AreDistinctPowersOfTwo(ExcludedControl flag, int expectedValue)
        {
            Assert.AreEqual(expectedValue, (int)flag);
        }

        #endregion

        #region Combination

        [Test]
        public void FlagsCanBeCombined_AndCheckedWithHasFlag()
        {
            var combo =
                ExcludedControl.MousePosition
                | ExcludedControl.MouseDelta
                | ExcludedControl.TouchPosition;

            Assert.IsTrue(combo.HasFlag(ExcludedControl.MousePosition));
            Assert.IsTrue(combo.HasFlag(ExcludedControl.MouseDelta));
            Assert.IsTrue(combo.HasFlag(ExcludedControl.TouchPosition));
            Assert.IsFalse(combo.HasFlag(ExcludedControl.MouseScroll));
            Assert.IsFalse(combo.HasFlag(ExcludedControl.PointerPosition));
        }

        #endregion
    }
}
