using System.Globalization;
using AirTransfer.Extensions;
using AirTransfer.Interfaces;
using AirTransfer.Resources.Languages;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Setting : PageComponentBase
{
    #region Injects

    [Inject] private IUserService UserService { get; set; } = null!;

    [Inject] private NavigationManager NavigationManager { get; set; } = null!;

    [Inject] private ILanguageService LanguageService { get; set; } = null!;

    #endregion

    #region Parameters

    [Parameter] public List<KeyValuePair<string, string>> Languages { get; set; } = [];

    [Parameter] public string SelectedLanguage { get; set; } = string.Empty;
    #endregion

    #region Overrides

    protected override Task OnPageInitializedAsync(string? url, Dictionary<string, object>? data)
    {
        InitLanguages();

        return base.OnPageInitializedAsync(url, data);
    }

    #endregion

    #region Commands

    private Task SaveLanguageCommand(KeyValuePair<string, string> selectedLanguage)
    {
        LanguageService.SetLanguage(selectedLanguage.Value);
        Localizer.SetCulture(new(selectedLanguage.Value));
        return Task.CompletedTask;
    }

    private async Task LogoutCommand()
    {
        await UserService.ClearUserAsync();
        //清空所有的任务

        var devices = StateManager.Devices();

        foreach (var device in devices)
            if (device.TransmissionTasks.Any())
                foreach (var task in device.TransmissionTasks)
                    if (task.CancellationTokenSource != null)
                        await task.CancellationTokenSource.CancelAsync();

        // 状态字典全部清空
        StateManager.Cleaer();

        NavigationManager.NavigateTo("/");
    }

    #endregion


    #region Private methods

    private void InitLanguages()
    {
        Languages.Add(new("English", "en-US"));
        Languages.Add(new("中文", "zh-hans"));
        Languages.Add(new("日本語", "ja-JP"));
        Languages.Add(new("한국어", "ko-KR"));
        Languages.Add(new("français", "fr-FR"));

        SelectedLanguage = LanguageService.GetLanguage() ?? throw new ArgumentNullException(nameof(LanguageService));

    }

    #endregion
}