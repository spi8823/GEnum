using NuGet.Frameworks;
using System.Text;

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

        #region Displaying
        [TestMethod]
        public void TestGetDefineName()
        {
            Assert.AreEqual(TestFlag.None.GetDefineName(), "None");
            Assert.AreEqual(TestFlag.A.GetDefineName(), "A");
            Assert.AreEqual(TestFlag.B.GetDefineName(), "B");
            Assert.AreEqual((TestFlag.A | TestFlag.B).GetDefineName(), "Undefined");
        }

        public void TestGetDisplayName()
        {
            Assert.AreEqual(TestFlag.None.GetDisplayName(), "None");
            Assert.AreEqual(TestFlag.A.GetDisplayName(), "DisplayA");
            Assert.AreEqual((TestFlag.A | TestFlag.B).GetDisplayName(), "DisplayA | DisplayB");
        }
        #endregion
    }
}
