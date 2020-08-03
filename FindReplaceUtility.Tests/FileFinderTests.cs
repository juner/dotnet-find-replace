using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FindReplaceUtility.Tests
{
    [TestClass()]
    public class FileFinderTests
    {
        static IEnumerable<object[]> DeconstructTestData
        {
            get
            {
                {
                    var Directory = new DirectoryInfo(".");
                    var Include = new Regex(".*");
                    var Exclude = new Regex("[.]git");
                    yield return DeconstructTestData(
                        new FileFinder(), Directory, Include, Exclude);
                }
                {
                    var Directory = new DirectoryInfo(".");
                    var Include = new Regex(".*");
                    var Exclude = new Regex("[.]git");
                    yield return DeconstructTestData(
                        new FileFinder(Directory, Include, Exclude), Directory, Include, Exclude);
                }
                static object[] DeconstructTestData(FileFinder FileFinder, DirectoryInfo ExpectDirectory, Regex ExpectInclude, Regex ExpectExclude)
                    => new object[] { FileFinder, ExpectDirectory, ExpectInclude, ExpectExclude };
            }
        }
        [TestMethod()]
        [DynamicData(nameof(DeconstructTestData))]
        public void DeconstructTest(FileFinder FileFinder, DirectoryInfo ExpectDirectory, Regex ExpectInclude, Regex ExpectExclude)
        {
            var (Directory, Include, Exclude) = FileFinder;
            Assert.AreEqual(ExpectDirectory.FullName, Directory.FullName);
            Assert.AreEqual(ExpectInclude.ToString(), Include.ToString());
            Assert.AreEqual(ExpectExclude.ToString(), Exclude.ToString());
        }

        [TestMethod()]
        public void FilesTest()
        {
            var tmp = Directory.CreateDirectory("tmp");
            using var _tmp = Disposable.Create(() => tmp.Delete(true));
            var FullName = Path.Combine(tmp.FullName, "text.txt");
            File.WriteAllText(FullName, "hello");
            var expected = new[] { new FileInfo(FullName) };
            var ff = new FileFinder
            {
                Directory = tmp,
                Include = new Regex(".*"),
                Exclude = new Regex("[.]git"),
            };
            var files = ff.Files().ToArray();
            CollectionAssert.AreEqual(expected.Select(v => v.FullName).ToList(), files.Select(v => v.FullName).ToList());

        }
        static IEnumerable<object[]> DoMatchingTestData
        {
            get
            {
                yield return DoMatchingTestData(
                    new FileFinder
                    {
                        Exclude = new Regex(".*"),
                        Include = new Regex(".*"),
                    }, "test.txt", false);
                static object[] DoMatchingTestData(FileFinder FileFinder, string Path, bool Expected)
                    => new object[] { FileFinder, Path, Expected };
            }
        }
        [TestMethod()]
        [DynamicData(nameof(DoMatchingTestData))]
        public void DoMatchingTest(FileFinder FileFinder, string Path, bool Expected)
            => Assert.AreEqual(Expected, FileFinder.DoMatching(Path));
    }
}
