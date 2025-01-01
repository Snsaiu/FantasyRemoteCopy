using AirTransfer.Models;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Components;

public partial class AiSetting : ComponentBase
{

    private List<AiApiModelBase> aiModels = [];
    private AiApiModelBase selectedAiModel;

    protected override void OnInitialized()
    {
        aiModels.Add(new OpenAiApiModel());
        selectedAiModel = aiModels.First();
    }
    
    private void SelectedOptionChangedCommand(AiApiModelBase model)
    {
        selectedAiModel = model;
     
    }
}