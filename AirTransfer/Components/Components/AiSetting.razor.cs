using AirTransfer.Interfaces.IConfigs;
using AirTransfer.Models;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Components;

public partial class AiSetting : ComponentBase
{

    private List<AiApiModelBase> aiModels = [];
    private AiApiModelBase selectedAiModel;

    [Inject] protected IAiSettingService AiService { get; set; } = null!;

    protected override void OnInitialized()
    {
        var m = AiService.Query();
        if (m is null)
            aiModels.Add(new OpenAiApiModel());
        else
            aiModels.Add(m);
        selectedAiModel = aiModels.First();
    }

    private void SelectedOptionChangedCommand(AiApiModelBase model)
    {
        selectedAiModel = model;
    }

    private void SaveCommand()
    {
        AiService.Save(selectedAiModel as OpenAiApiModel);
    }
}