using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;

using System.Globalization;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class SettingPageModel : ViewModelBase
{
    private readonly IDialogService _dialogService;
    private readonly DeviceLocalIpBase _getLocalIp;
    private readonly GlobalScanBase _globalScan;
    private readonly FileSavePathBase fileSavePath;
    private readonly ILanguageService _languageService;
    private readonly INavigationService _navigationService;

    private readonly IUserService _userService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(LogoutCommand))]
    [NotifyCanExecuteChangedFor(nameof(GlobalSearchCommand))]
    private bool isBusy;

    [ObservableProperty] private List<KeyValuePair<string, string>> languages = [];

    [ObservableProperty] private KeyValuePair<string, string>? selectedLanguage;

    /// <inheritdoc />
    public SettingPageModel(IUserService userService,
        DeviceLocalIpBase getLocalIp,
        INavigationService navigationService,
        IDialogService dialogService,
        GlobalScanBase globalScan,
        FileSavePathBase fileSavePath,
        ILanguageService languageService)
    {
        _userService = userService;
        _getLocalIp = getLocalIp;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _globalScan = globalScan;
        this.fileSavePath=fileSavePath;
        _languageService = languageService;

        InitLanguages();
    }

    public bool IsNotBusy => !IsBusy;

    [ObservableProperty]
    private string savePath;

    [ObservableProperty]
    private bool showChangedFolder;

    private void InitLanguages()
    {
        Languages.Add(new KeyValuePair<string, string>("English", "en-US"));
        Languages.Add(new KeyValuePair<string, string>("中文", "zh-hans"));
        Languages.Add(new KeyValuePair<string, string>("日本語", "ja-JP"));
        Languages.Add(new KeyValuePair<string, string>("한국어", "ko-KR"));
        Languages.Add(new KeyValuePair<string, string>("français", "fr-FR"));
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task GlobalSearchAsync()
    {
        try
        {
            IsBusy = true;
            string ip = await _getLocalIp.GetLocalIpAsync();
            await _globalScan.SendAsync(ip, default);
        }
        finally
        {
            IsBusy = false;
            await _dialogService.DisplayAlert("Information", "Search Complete !", "Ok");
        }
    }

    [RelayCommand(CanExecute = nameof(IsNotBusy))]
    private async Task Logout()
    {
        await _userService.ClearUserAsync();
        await _navigationService.NavigationToAsync(nameof(LoginPage), false);
    }


    [RelayCommand]
    private async Task ChangedSavePathAsync()
    {
        var result = await FolderPicker.Default.PickAsync();
        if (result is not { IsSuccessful:true,Folder.Path :var path})
        {
            return;
        }
        ((IChangePathable)fileSavePath).ChangedPath(path);
        SavePath= path;

    }


    [RelayCommand]
    private void Init()
    {
        string? language = _languageService.GetLanguage();
        SelectedLanguage = string.IsNullOrEmpty(language)
            ? Languages.First()
            : Languages.First(x => x.Value == language);

       SavePath = fileSavePath.SaveLocation;

        ShowChangedFolder = fileSavePath is IChangePathable;
    }

    partial void OnSelectedLanguageChanged(KeyValuePair<string, string>? oldValue, KeyValuePair<string, string>? newValue)
    {
        if (oldValue is null)
            return;
        if (newValue is null)
            return;
        _languageService.SetLanguage(newValue.Value.Value);
        LocalizationResource.SetCulture(new CultureInfo(newValue.Value.Value));
    }

}