using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Login : ComponentBase
{
    [Parameter] public string? LoginName { get; set; }

    [Parameter] public string? DeviceName { get; set; }


    private Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(LoginName) || string.IsNullOrWhiteSpace(DeviceName))
        {
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}