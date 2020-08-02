using System.Collections.Generic;
using System.IO;
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
            Assert.AreEqual(ExpectDirectory, Directory);
            Assert.AreEqual(ExpectInclude, Include);
            Assert.AreEqual(ExpectExclude, Exclude);
        }

        [TestMethod()]
        public void FilesTest()
        {
            Assert.Fail();
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
