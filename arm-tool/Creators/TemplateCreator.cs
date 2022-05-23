using armtool.Common;
using armtool.Common.Templates;
using Newtonsoft.Json.Linq;

namespace armtool.Creators;

internal class TemplateCreator
{
    public NestedTemplateResource CreateNestedTemplateResource(string name, string[] dependsOn, JObject nestedTemplate, IEnumerable<KeyValuePair<string, TemplateParameterProperties>> parameters)
    {
        var nestedParameters = new Dictionary<string, TemplateParameterProperties>();
        foreach (var p in parameters)
        {
            nestedParameters.Add(p.Key, new TemplateParameterProperties
            {
                value = $"[parameters('{p.Key}')]"
            });
        }

        NestedTemplateResource nestedTemplateResource = new NestedTemplateResource()
        {
            name = name,
            type = "Microsoft.Resources/deployments",
            apiVersion = GlobalConstants.LinkedAPIVersion,
            properties = new NestedTemplateProperties()
            {
                mode = "Incremental",
                expressionEvaluationOptions = new NestedExpressionEvaluationOptions
                {
                    scope = "inner"
                },
                template = nestedTemplate,
                parameters = nestedParameters
            },
            dependsOn = dependsOn
        };
        return nestedTemplateResource;
    }

    public Template CreateEmptyTemplate()
    {
        // creates empty template for use in all other template creators
        Template template = new Template()
        {
            schema = "https://schema.management.azure.com/schemas/2019-04-01/deploymentTemplate.json#",
            contentVersion = "1.0.0.0",
            parameters = { },
            variables = { },
            resources = new TemplateResource[] { },
            outputs = { }
        };
        return template;
    }
}