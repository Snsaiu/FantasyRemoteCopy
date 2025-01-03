using AirTransfer.Interfaces.IConfigs;
using AirTransfer.Models;
using Newtonsoft.Json;

namespace AirTransfer.Interfaces.Impls.Configs;

public sealed class AiSettingService : ConfigServiceBase, IAiSettingService
{
    public override string Key { get; } = "OpenAiApi";

    public void Save(OpenAiApiModel model)
    {
        var json = JsonConvert.SerializeObject(model);
        Set(json);
    }

    public OpenAiApiModel? Query()
    {
        var json = Get<string>();
        if (string.IsNullOrEmpty(json))
            return null;
        return JsonConvert.DeserializeObject<OpenAiApiModel>(json);
    }
}