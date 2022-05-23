using Armtool.Commands;
using Armtool.Common;
using Microsoft.Extensions.CommandLineUtils;

namespace Armtool;

internal class Program
{
    static int Main(string[] args)
    {

        var app = new CommandLineApplication()
        {
            Name = GlobalConstants.AppShortName,
            FullName = GlobalConstants.AppLongName,
            Description = GlobalConstants.AppDescription
        };

        app.Command("nest", command =>
        {
            command.Name = GlobalConstants.NestName;
            command.Description = GlobalConstants.NestDescription;

            var inputFilesOption = command.Option("--inputFile <inputFile>", "ARM template input", CommandOptionType.MultipleValue);
            var outputFileOption = command.Option("--outputFile <outputFile>", "Nested ARM template output", CommandOptionType.SingleValue);

            command.OnExecute(() =>
            {
                var exitCode = new NestCommand(inputFilesOption.Values, outputFileOption.Value()).Run();

                return exitCode;
            });
        });

        try
        {
            return app.Execute(args);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            Console.Out.WriteLine(e.ToString());
        }

        return 1;
    }
}