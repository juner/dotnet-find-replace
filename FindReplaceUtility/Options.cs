using CommandLine;

namespace FindReplaceUtility
{
    public class Options
    {
        [Value(0, Default = ".", HelpText = "Current direcotry.", Required = false)]
        public string Current { get; set; } = string.Empty;
        [Option('l', "list", Default = false, HelpText = "list target file. no execute.", Required = false)]
        public bool IsList { get; set; }
        [Option('n', "no-result-is-success", Default = false, HelpText = "no replace result is success.", Required = false)]
        public bool NoResultIsSuccess { get; set; }
        [Option('i', "include", Default = ".*", HelpText = "A regular expression of files to include in our find and replace", Required = false)]
        public string Include { get; set; } = string.Empty;
        [Option('e', "exclude", Default = "[.]git", HelpText = "A regular expression of files to exclude in our find and replace", Required = false)]
        public string Exclude { get; set; } = string.Empty;
        [Option('f', "find", HelpText = "The string we want to replace", Required = true)]
        public string Find { get; set; } = string.Empty;
        [Option('r', "replace", HelpText = "The new string to replace with", Required = true)]
        public string Replace { get; set; } = string.Empty;
        public void Deconstruct(out string Current, out string Include, out string Exclude, out string Find, out string Replace)
            => (Current, Include, Exclude, Find, Replace)
            = (this.Current, this.Include, this.Exclude, this.Find, this.Replace);
    }
}
