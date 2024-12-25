using System.Globalization;

using AirTransfer.Extensions;
using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls.Configs;
using AirTransfer.Resources.Languages;

using CommunityToolkit.Maui.Storage;

using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class Setting : PageComponentBase
{
    #region Injects

    [Inject] private IUserService UserService { get; set; } = null!;

    [Inject] private FileSavePathBase FileSavePath { get; set; } = null!;

    [Inject] private ILanguageService LanguageService { get; set; } = null!;

    [Inject] private IThemeService ThemeService { get; set; } = null!;

    #endregion

    #region Parameters

    public List<KeyValuePair<string, string>> Languages { get; set; } = [];


    [Parameter] public DesignThemeModes Theme { get; set; }

    [Parameter] public OfficeColor? OfficeColor { get; set; }

    private string SavePath { get; set; } = String.Empty;

    private bool IsClipboardWatch { get; set; } = false;

    [Parameter] public string SelectedLanguage { get; set; } = string.Empty;
    #endregion

    #region Overrides

    protected override Task OnParametersSetAsync()
    {
        Theme = ThemeService.GetDesignTheme();
        OfficeColor = ThemeService.GetThemeColor();
        SavePath = FileSavePath.SaveLocation;
        return base.OnParametersSetAsync();
    }

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
        ToastService.ShowInfo(Localizer["SetLanguageInfo"]);
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


    private void SaveThemeModeCommand(DesignThemeModes designThemeModes)
    {
        Theme = designThemeModes;
        ThemeService.SetDesignTheme(designThemeModes);
    }

    private void SaveColorCommand(OfficeColor? officeColor)
    {
        OfficeColor = officeColor;
        ThemeService.SetThemeColor(officeColor!.Value);
    }


    private async Task SavePathCommand()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (result is not { IsSuccessful: true, Folder.Path: var path })
        {
            return;
        }
        ((IChangePathable)FileSavePath).ChangedPath(path);
        SavePath = path;
    }

    private void ClipboardWatchChangedCommand()
    {
        LoopWatchClipboardService.SetState(IsClipboardWatch);
        ToastService.ShowSuccess(Localizer["UpdateClipboardStateMessage"]);
    }
}