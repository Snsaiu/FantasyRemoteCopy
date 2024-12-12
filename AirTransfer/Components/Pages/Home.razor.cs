using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Home : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Parameter] public string Text { get; set; }


    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    private void GotoTextInputPage()
    {
        this.NavigationManager.NavigateTo("/Home/TextInput");
    }
}