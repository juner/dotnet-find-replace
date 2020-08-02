using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LazyRegex = System.Lazy<System.Text.RegularExpressions.Regex>;

namespace FindReplaceUtility
{
    public class FindReplace
    {
        readonly LazyRegex include;
        readonly LazyRegex exclude;
        readonly LazyRegex find;
        public DirectoryInfo Directory { get; }
        public Regex Include => include.Value;
        public Regex Exclude => exclude.Value;
        public Regex Find => find.Value;
        public string Replace { get; }
        readonly EnumerationOptions EnumerationOptions;
        static EnumerationOptions DefaultEnumerationOptions => new EnumerationOptions
        {
            AttributesToSkip = 0,
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
        };
        public void Deconstruct(out DirectoryInfo Directory, out Regex Include, out Regex Exclude, out Regex Find, out string Replace)
            => (Directory, Include, Exclude, Find, Replace)
            = (this.Directory, this.Include, this.Exclude, this.Find, this.Replace);
        private LazyRegex ToLazyRegex(string Pattern) => new LazyRegex(() => new Regex(Pattern));
        private LazyRegex ToLazyRegex(Regex Regex) => new LazyRegex(Regex);
        private DirectoryInfo ToDirectory(string Path) => new DirectoryInfo(Path);
        public FindReplace(string Path, string Include, string Exclude, string Find, string Replace)
            => (Directory, include, exclude, find, this.Replace, EnumerationOptions)
            = (ToDirectory(Path), ToLazyRegex(Include), ToLazyRegex(Exclude), ToLazyRegex(Find), Replace, DefaultEnumerationOptions);
        public FindReplace(string Path, Regex Include, Regex Exclude, Regex Find, string Replace)
            => (this.Directory, include, exclude, find, this.Replace, EnumerationOptions)
            = (ToDirectory(Path), ToLazyRegex(Include), ToLazyRegex(Exclude), ToLazyRegex(Find), Replace, DefaultEnumerationOptions);
        public IEnumerable<FileInfo> Files()
        {
            return Directory
                .EnumerateFiles("*", EnumerationOptions)
                .Where(DoMatching);
        }
        public bool DoMatching(FileInfo File) => Include.IsMatch(File.FullName) && !Exclude.IsMatch(File.FullName);
        public bool DoMatching(string Path) => Include.IsMatch(Path) && !Exclude.IsMatch(Path);
        public async ValueTask<bool> FindAndReplaceAsync(string Path, bool IsWrite = true, CancellationToken Token = default)
        {
            var Input = await File.ReadAllTextAsync(Path, Token);
            var Output = Find.Replace(Input, Replace);
            if (Input == Output)
                return false;
            if (IsWrite)
                await File.WriteAllTextAsync(Path, Output, Token);
            return true;
        }
    }
}
