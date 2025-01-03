using AirTransfer.Models;

namespace AirTransfer.Interfaces.IConfigs;

public interface IAiSettingService : IConfigService
{
    void Save(OpenAiApiModel model);
    OpenAiApiModel? Query();
}