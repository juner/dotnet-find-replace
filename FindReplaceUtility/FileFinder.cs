using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LazyRegex = System.Lazy<System.Text.RegularExpressions.Regex>;

namespace FindReplaceUtility
{
    public class FileFinder
    {
        internal const string DefaultPath = ".";
        internal const string DefaultInclude = ".*";
        internal const string DefaultExclude = "[.]git";
        public DirectoryInfo Directory { get; set; }
        public Regex Include
        {
            get => include.Value;
            set => include = ToLazyRegex(value);
        }
        public Regex Exclude
        {
            get => exclude.Value;
            set => exclude = ToLazyRegex(value);
        }

        private LazyRegex include;
        private LazyRegex exclude;
        readonly EnumerationOptions EnumerationOptions;
        static EnumerationOptions DefaultEnumerationOptions => new EnumerationOptions
        {
            AttributesToSkip = 0,
            IgnoreInaccessible = true,
            RecurseSubdirectories = true,
        };
        private DirectoryInfo ToDirectory(string Path) => new DirectoryInfo(Path);
        private LazyRegex ToLazyRegex(string Pattern) => new LazyRegex(() => new Regex(Pattern));
        private LazyRegex ToLazyRegex(Regex Regex) => new LazyRegex(Regex);
        public FileFinder() : this(DefaultPath, DefaultInclude, DefaultExclude) { }
        public FileFinder(string Path, string Include, string Exclude)
            => (Directory, include, exclude, EnumerationOptions)
            = (ToDirectory(Path), ToLazyRegex(Include), ToLazyRegex(Exclude), DefaultEnumerationOptions);
        public FileFinder(DirectoryInfo Directory, Regex Include, Regex Exclude)
            => (this.Directory, include, exclude, EnumerationOptions)
            = (Directory, ToLazyRegex(Include), ToLazyRegex(Exclude), DefaultEnumerationOptions);
        public void Deconstruct(out DirectoryInfo Directory, out Regex Include, out Regex Exclude)
            => (Directory, Include, Exclude)
            = (this.Directory, this.Include, this.Exclude);
        public IEnumerable<FileInfo> Files()
        {
            return Directory
                .EnumerateFiles("*", EnumerationOptions)
                .Where(DoMatching);
        }
        public bool DoMatching(FileInfo File) => DoMatching(File.FullName);
        public bool DoMatching(string Path) => Include.IsMatch(Path) && !Exclude.IsMatch(Path);
        public override string ToString()
            => nameof(FileFinder) + "{" + string.Join(", ", new[] {
                $"{nameof(Directory)}:{Directory}",
                $"{nameof(Include)}:{Include}",
                $"{nameof(Exclude)}:{Exclude}",
            }) + "}";
    }
}
