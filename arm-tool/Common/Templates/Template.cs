using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Armtool.Common.Templates;

internal class Template
{
    [JsonProperty(PropertyName = "$schema")]
    public string schema { get; set; }
    public string contentVersion { get; set; }
    public Dictionary<string, TemplateParameterProperties> parameters { get; set; }
    public object variables { get; set; }
    public TemplateResource[] resources { get; set; }
    public object outputs { get; set; }
}

internal class TemplateResource
{
    public TemplateProperties properties { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public string apiVersion { get; set; }
    public string scale { get; set; }
    public string[] dependsOn { get; set; }
}

internal class TemplateProperties
{
    public string mode { get; set; }
    public TemplateLink templateLink { get; set; }
    public Dictionary<string, TemplateParameterProperties> parameters { get; set; }
}

internal class TemplateLink
{
    public string uri { get; set; }
    public string contentVersion { get; set; }
}

internal class NestedTemplateResource : TemplateResource
{
    public NestedTemplateProperties properties { get; set; }
}

internal class NestedTemplateProperties
{
    public string mode { get; set; }

    public NestedExpressionEvaluationOptions expressionEvaluationOptions { get; set; }

    public JObject template { get; set; }

    public Dictionary<string, TemplateParameterProperties> parameters { get; set; }
}

internal class NestedExpressionEvaluationOptions
{
    public string scope { get; set; }
}


internal class TemplateParameterProperties
{
    public string type { get; set; }
    public TemplateParameterMetadata metadata { get; set; }
    public string[] allowedValues { get; set; }
    public string defaultValue { get; set; }
    public string value { get; set; }
}

internal class TemplateParameterMetadata
{
    public string description { get; set; }
}