using AirTransfer.Interfaces;
using AirTransfer.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Fast.Components.FluentUI;

namespace AirTransfer.Components.Pages;

public partial class Login : ComponentBase
{
    #region MyRegion

    [Parameter] public string? UserName { get; set; }

    [Parameter] public string? DeviceNiceName { get; set; }

    [Parameter] public bool IsBusy { get; set; }

    #endregion Parameters


    #region Injects

    [Inject] private NavigationManager? NavigationManager { get; set; }

    [Inject] private IUserService? UserService { get; set; }


    [Inject] private IDialogService? DialogService { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        if (UserService is null)
            throw new NullReferenceException();
        try
        {
            this.IsBusy = true;
            var userResult = await UserService.GetCurrentUserAsync();
            if (userResult.Ok)
            {
                NavigationManager?.NavigateTo("/home");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// 登陆
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(UserName))
        {
            DialogService?.ShowError("用户名称不能为空", "错误");
            return;
        }

        if (string.IsNullOrWhiteSpace(DeviceNiceName))
        {
            DialogService?.ShowError("设备昵称不能为空", "错误");
            return;
        }

        if (UserService is null)
            throw new NullReferenceException(nameof(UserService));
        this.IsBusy = true;
        try
        {
            await UserService.SaveUserAsync(new UserInfo() { Name = UserName, DeviceNickName = DeviceNiceName });
        }
        finally
        {
            IsBusy = false;
        }

        NavigationManager?.NavigateTo($"/home");
    }
}