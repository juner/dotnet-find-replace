using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using LazyRegex = System.Lazy<System.Text.RegularExpressions.Regex>;

namespace FindReplaceUtility
{
    public class FindReplace
    {
        private LazyRegex? find;
        public Regex Find
        {
            get => find?.Value ?? throw new InvalidOperationException(nameof(Find) + " is no set.");
            set => find = ToLazyRegex(value);
        }
        private string? replace;
        public string Replace
        {
            get => replace ?? throw new InvalidOperationException(nameof(Replace) + " is no set.");
            set => replace = value;
        }
        public void Deconstruct(out Regex Find, out string Replace)
            => (Find, Replace)
            = (this.Find, this.Replace);
        private LazyRegex ToLazyRegex(string Pattern) => new LazyRegex(() => new Regex(Pattern));
        private LazyRegex ToLazyRegex(Regex Regex) => new LazyRegex(Regex);
        public FindReplace() { }
        public FindReplace(string Find, string Replace)
            => (find, this.Replace)
            = (ToLazyRegex(Find), Replace);
        public FindReplace(Regex Find, string Replace)
            => (find, this.Replace)
            = (ToLazyRegex(Find), Replace);

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
