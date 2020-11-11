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
        static IEnumerable<object?[]> FindAndReplaceAsyncTestData
        {
            get
            {
                yield return FindAndReplaceAsyncTest(
                    "hello", new Regex("hello"), "world",
                    true, "world"
                );
                yield return FindAndReplaceAsyncTest(
                    "hello \r\n hello \r\n world!", new Regex("hello"), "super",
                    true, "super \r\n super \r\n world!"
                );
                yield return FindAndReplaceAsyncTest(
                    "no maches test \r\n ?", new Regex("test"), "mache?",
                    false, null
                );
                static object?[] FindAndReplaceAsyncTest(string FileText, Regex Find, string Replace, bool ExpectedResult, string? ExpectedFileText)
                    => new object?[] { FileText, Find, Replace, ExpectedResult, ExpectedFileText };
            }
        }
        [TestMethod, DynamicData(nameof(FindAndReplaceAsyncTestData))]
        public async Task FindAndReplaceAsyncTest(string FileText, Regex Find, string Replace, bool ExpectedResult, string? ExpectedFileText)
        {
            using var tmp = new TempDirectory();
            var FullName = Path.Combine(tmp.FullName, "text.txt");
            File.WriteAllText(FullName, "hello");
            var expected = new[] { new FileInfo(FullName) };
            var fr = new FindReplace
            {
                Find = new Regex("hello"),
                Replace = "world",
            };
            var Result = await fr.FindAndReplaceAsync(FullName);
            Assert.AreEqual(ExpectedResult, Result);
            if (Result)
                Assert.AreEqual(ExpectedFileText, await File.ReadAllTextAsync(FullName));
        }
    }
}
