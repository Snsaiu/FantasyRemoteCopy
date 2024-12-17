using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class TextInput : PageComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public string? Text { get; set; }


    private void SubmitCommand()
    {
        this.NavigateTo("/home", new Dictionary<string, object> { { "text", Text } });
    }
}