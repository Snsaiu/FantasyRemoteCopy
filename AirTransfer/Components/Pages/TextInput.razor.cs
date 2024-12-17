using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class TextInput : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public string? Text { get; set; }


    private void SubmitCommand()
    {
        var p = NavigationManager.GetUriWithQueryParameter("/home", Text);
        NavigationManager.NavigateTo($"/home?data={Text}");
    }
}