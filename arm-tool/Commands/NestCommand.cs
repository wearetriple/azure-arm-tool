using Armtool.Common.FileHandlers;
using Armtool.Common.Templates;
using Armtool.Creators;
using Newtonsoft.Json.Linq;

namespace Armtool.Commands;

internal class NestCommand
{
    private readonly List<string> _inputFiles;
    private readonly string _outputFile;

    public NestCommand(List<string> inputFiles, string outputFile)
    {
        _inputFiles = inputFiles;
        _outputFile = outputFile;
    }

    public int Run()
    {
        try
        {
            var inputFilesData = _inputFiles.Select(file =>
            {
                using var fileStream = File.OpenRead(file);
                using var fileReader = new StreamReader(fileStream);

                return (fileName: Path.GetFileName(file).Replace(Path.GetExtension(file), ""), json: fileReader.ReadToEnd());
            });

            var templates = inputFilesData.Select(data => (data.fileName, template: JObject.Parse(data.json))).ToList();

            if (templates.Count == 0)
            {
                throw new InvalidOperationException("No JSON files found.");
            }

            var templateCreator = new TemplateCreator();

            var linkedTemplate = templateCreator.CreateEmptyTemplate();
            linkedTemplate.parameters = new Dictionary<string, TemplateParameterProperties>();

            var resources = new List<NestedTemplateResource>();
            var dependsOn = new List<string>();

            var fileWriter = new FileWriter();

            foreach ((string fileName, JObject template) in templates)
            {
                var parameters = template.GetValue("parameters").ToObject<Dictionary<string, TemplateParameterProperties>>();

                var resource = templateCreator.CreateNestedTemplateResource(
                    fileName,
                    dependsOn.ToArray(),
                    template,
                    parameters);


                foreach (var parameter in parameters)
                {
                    if (!linkedTemplate.parameters.ContainsKey(parameter.Key))
                    {
                        linkedTemplate.parameters.Add(parameter.Key, parameter.Value);
                    }
                }

                dependsOn.Add($"[resourceId('Microsoft.Resources/deployments', '{fileName}')]");
                resources.Add(resource);
            }

            linkedTemplate.resources = resources.ToArray();

            fileWriter.WriteJSONToFile(linkedTemplate, _outputFile);

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error occured: " + ex.Message);
            throw;
        }
    }
}
