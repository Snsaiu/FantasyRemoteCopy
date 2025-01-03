using AirTransfer.Interfaces;

namespace AirTransfer.Models;


public abstract class AiApiModelBase
{
    // 模型提供方
    public abstract string Provider { get; }

    public int ContextCount { get; set; }

    public bool ContextCountLimit { get; set; }

    public double Temperature { get; set; }

}

public abstract class AiApiKeyModelBase : AiApiModelBase,IApiKey
{
    public string ApiKey { get; set; }
}

public class OpenAiApiModel : AiApiKeyModelBase, IHasCustomModel, IApiPath, ITopP, IApiDomain
{
    public string SelectedModel { get; set; }
    public IEnumerable<string> GetModels()
    {
        return new List<string> {"chatgpt-4o-latest", "gpt-3.5-turbo"};
    }

    public string? CustomModelName { get; set; }

    public double TopP { get; set; }
    public string? ApiPath { get; set; }
    public override string Provider => "OpenAI API";
    public string ApiDomain { get; set; }
}