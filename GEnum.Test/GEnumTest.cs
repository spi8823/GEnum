using NuGet.Frameworks;

namespace GEnum.Test
{
    [TestClass]
    public class GEnumTest
    {
        #region Flags
        [TestMethod]
        public void TestContains()
        {
            var flag = TestFlag.A | TestFlag.B;
            Assert.IsTrue(flag.Contains(TestFlag.A));
            Assert.IsTrue(flag.Contains(TestFlag.B));
            Assert.IsFalse(flag.Contains(TestFlag.C));
        }

        [TestMethod]
        public void TestAdd()
        {
            var flag = TestFlag.A;
            flag.Add(TestFlag.C);
            Assert.IsTrue(flag.Contains(TestFlag.A));
            Assert.IsFalse(flag.Contains(TestFlag.B));
            Assert.IsTrue(flag.Contains(TestFlag.C));
        }

        [TestMethod]
        public void TestRemove()
        {
            var flag = TestFlag.A | TestFlag.B;
            flag.Remove(TestFlag.B);

            Assert.IsTrue(flag.Contains(TestFlag.A));
            Assert.IsFalse(flag.Contains(TestFlag.B));
            Assert.IsFalse(flag.Contains(TestFlag.C));
        }

        [TestMethod]
        public void TestClear()
        {
            var flag = TestFlag.A | TestFlag.B;
            flag.Clear();

            Assert.IsFalse(flag.Contains(TestFlag.A));
            Assert.IsFalse(flag.Contains(TestFlag.B));
            Assert.IsFalse(flag.Contains(TestFlag.C));
        }
        #endregion

        #region Matching
        [TestMethod]
        public void TestIs()
        {
            var flag = TestFlag.A;

            Assert.IsTrue(flag.IsA());
            Assert.IsFalse(flag.IsB());
            Assert.IsFalse(flag.IsC());
        }
        #endregion
    }
}