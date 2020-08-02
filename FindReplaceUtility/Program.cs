using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;

namespace FindReplaceUtility
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            using var CancellationTokenSource = new CancellationTokenSource();
            void CancelKeyPress(object sender, ConsoleCancelEventArgs args)
            {
                if (args.Cancel)
                    CancellationTokenSource.Cancel();
            }
            Console.CancelKeyPress += CancelKeyPress;
            var result = new Parser(settings =>
            {
                settings.IgnoreUnknownArguments = false;
            }).ParseArguments<Options>(args);
            return await result.MapResult(
                option => Excute(option, CancellationTokenSource.Token), _ => Errors(result));
        }
        public static Task<int> Excute(Options Options, CancellationToken Token = default)
        {
            Console.WriteLine("Hello!");
            var (Current, IsList, Include, Exclude, Find, Replace) = Options;
            Console.WriteLine($"{nameof(Current)}:{Current}");
            Console.WriteLine($"{nameof(IsList)}:{IsList}");
            Console.WriteLine($"{nameof(Include)}:{Include}");
            Console.WriteLine($"{nameof(Exclude)}:{Exclude}");
            Console.WriteLine($"{nameof(Find)}:{Find}");
            Console.WriteLine($"{nameof(Replace)}:{Replace}");
            var fr = new FindReplace(Current, Include, Exclude, Find, Replace);
            return Excute();
            async Task<int> Excute()
            {
                try
                {
                    foreach (var FileInfo in fr.Files())
                        try
                        {
                            Console.WriteLine(FileInfo);
                            var result = await fr.FindAndReplaceAsync(FileInfo.FullName, !IsList, Token);
                            if (result)
                                Console.WriteLine("! " + FileInfo);
                        }
                        catch (Exception e2)
                        {
                            Console.WriteLine(e2);
                        }
                    return 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 2;
                }
            }

        }
        public static Task<int> Errors(ParserResult<Options> Result)
        {
            var HelpText = CommandLine.Text.HelpText.AutoBuild(Result);
            Console.WriteLine(HelpText);
            return Task.FromResult(1);
        }

    }
}
