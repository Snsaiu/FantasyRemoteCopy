using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class TextInput : PageComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter] public string? Text { get; set; }


    private void SubmitCommand()
    {
        if (string.IsNullOrEmpty(Text))
        {
            this.ToastService.ShowError(Localizer["InputTextPlaceholder"]);
            return;
        }
        
        this.NavigateTo("/home", new Dictionary<string, object> { { "text", Text } });
    }

    private void ReturnCommand()
    {
        this.NavigateTo("/home");
    }
}