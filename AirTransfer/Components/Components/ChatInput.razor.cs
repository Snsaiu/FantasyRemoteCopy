using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Components;

public partial class ChatInput : ComponentBase
{
    [Parameter] public EventCallback<string> OnSend { get; set; }

    private string? text;

    private Task SendCommand()
    {
        OnSend.InvokeAsync(text);
        return Task.CompletedTask;
    }
}