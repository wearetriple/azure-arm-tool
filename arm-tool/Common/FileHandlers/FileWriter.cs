using Newtonsoft.Json;

namespace armtool.Common.FileHandlers;

internal class FileWriter
{
    public void WriteJSONToFile(object template, string location)
    {
        // writes json object to provided location
        string jsonString = JsonConvert.SerializeObject(template,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        File.WriteAllText(location, jsonString);
    }
}
