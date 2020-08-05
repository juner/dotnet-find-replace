using System;
using System.Net.Security;
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
                option => Execute(option, CancellationTokenSource.Token), _ => Errors(result));
        }
        const int SUCCESS_RESULT = 0;
        const int INFORMATION_RESULT = 1;
        const int NO_RESULT = 2;
        const int ERROR_RESULT = 3;
        public static Task<int> Execute(Options Options, CancellationToken Token = default)
        {
            var (Current, Include, Exclude, Find, Replace) = Options;
            var IsList = Options.IsNoReplaceMode;
            var NoResultIsSuccess = Options.NoResultIsSuccess;
            var ff = new FileFinder(Current, Include, Exclude);
            var fr = new FindReplace(Find, Replace);
            return Excute();
            async Task<int> Excute()
            {
                var Count = 0;
                try
                {
                    foreach (var FileInfo in ff.Files())
                        try
                        {
                            var result = await fr.FindAndReplaceAsync(FileInfo.FullName, !IsList, Token);
                            if (result)
                            {
                                Count++;
                                Console.WriteLine(FileInfo);
                            }
                        }
                        catch (Exception e2)
                        {
                            Console.Error.WriteLine(e2);
                        }
                    if (IsList)
                        return INFORMATION_RESULT;
                    Console.WriteLine($"Result:{Count}");
                    if (Count == 0 && !NoResultIsSuccess)
                        return NO_RESULT;
                    return SUCCESS_RESULT;
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    return ERROR_RESULT;
                }
            }

        }
        public static Task<int> Errors(ParserResult<Options> Result)
        {
            var HelpText = CommandLine.Text.HelpText.AutoBuild(Result);
            Console.WriteLine(HelpText);
            return Task.FromResult(INFORMATION_RESULT);
        }

    }
}
