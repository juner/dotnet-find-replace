using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FindReplaceUtility.Tests
{
    [TestClass()]
    public class FindReplaceTests
    {
        static IEnumerable<object[]> DeconstructTestData
        {
            get
            {
                var Find = new Regex("hello");
                var Replace = "world";
                yield return DeconstructTestData(new FindReplace(Find, Replace), Find, Replace);
                static object[] DeconstructTestData(FindReplace FindReplace, Regex Find, string Replace)
                    => new object[] { FindReplace, Find, Replace };
            }
        }
        [TestMethod()]
        [DynamicData(nameof(DeconstructTestData))]
        public void DeconstructTest(FindReplace FindReplace, Regex ExpectFind, string ExpectReplace)
        {
            var (Find, Replace) = FindReplace;
            Assert.AreEqual(ExpectFind.ToString(), Find.ToString());
            Assert.AreEqual(ExpectReplace, Replace);
        }
        [TestMethod()]
        public async Task FindAndReplaceAsyncTest()
        {
            using var tmp = new TempDirectory();
            var FullName = Path.Combine(tmp.FullName, "text.txt");
            File.WriteAllText(FullName, "hello");
            var ExpectedResult = true;
            var ExpectedFileText = "world";
            var expected = new[] { new FileInfo(FullName) };
            var fr = new FindReplace
            {
                Find = new Regex("hello"),
                Replace = "world",
            };
            var Result = await fr.FindAndReplaceAsync(FullName);
            var FileText = File.ReadAllText(FullName);
            Assert.AreEqual(ExpectedResult, Result);
            Assert.AreEqual(ExpectedFileText, FileText);

        }
    }
}
