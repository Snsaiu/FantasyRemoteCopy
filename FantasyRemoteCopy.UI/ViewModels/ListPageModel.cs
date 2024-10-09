using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.ViewModels.Base;
using FantasyRemoteCopy.UI.Views;

using System.Collections.ObjectModel;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class ListPageModel : ViewModelBase
{
    private readonly ISaveDataService _saveDataService;
    private readonly IOpenFolder _openFolder;

    [ObservableProperty]
    private ObservableCollection<SaveItemModel> models = [];

    [ObservableProperty]
    private bool isBusy;

    private readonly IDialogService _dialogService;

    private readonly INavigationService _navigationService;
    private readonly IOpenFileable openFileable;

    public ListPageModel(ISaveDataService saveDataService, IOpenFolder openFolder, IDialogService dialogService, INavigationService navigationService,IOpenFileable openFileable)
    {
        _saveDataService = saveDataService;
        _openFolder = openFolder;
        _dialogService = dialogService;
        _navigationService = navigationService;
        this.openFileable = openFileable;
    }


    private void rig(List<SaveDataModel> sources)
    {
        Models.Clear();
        foreach (SaveDataModel item in sources)
        {
            SaveItemModel sm = new SaveItemModel();
           
            if (item.DataType == SendType.Text)
            {
                sm.Title = item.Content.Replace(" ", "").Length > 20
                    ? item.Content.Replace(" ", "").Replace("\n", "")[..20] + "..."
                    : item.Content.Replace(" ", "").Replace("\n", "");
                //   sm.Image = ImageSource.FromFile("texticon.png");
            }
            else
            {
                sm.Title = Path.GetFileName(item.Content);
             //   sm.Image = ImageSource.FromFile("fileicon.png");
            }
            sm.SendType = item.DataType;
            sm.Content = item.Content;
            sm.Guid = item.Guid;
            sm.SourceDeviceName = item.SourceDeviceNickName;
            sm.Time = item.Time.ToString("yyyy-MM-dd HH:mm:ss");

            Models.Add(sm);
        }
    }


    [RelayCommand]
    private async Task CopyContent(SaveItemModel model)
    {
        await Clipboard.Default.SetTextAsync(model.Content);
        await _dialogService.DisplayAlert("Information", "Success copy!", "Ok");

    }


    [RelayCommand]
    private async Task OpenFile(SaveItemModel model)
    {
        if (string.IsNullOrEmpty(model.Content))
        {
            throw new NullReferenceException();
        }
        // 判断文件是否存在
        if (File.Exists(model.Content) == false)
        {
            FantasyResultModel.ResultBase<bool> res = await _saveDataService.DeleteDataAsync(model.Guid ?? throw new NullReferenceException());
            if (res.Ok)
            {
                Models.Remove(model);
            }
            else
            {
                await _dialogService.DisplayAlert("Warning", res.ErrorMsg, "Ok");
            }
        }
        
        openFileable.OpenFile(model.Content);
        
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    /// <param name="model"></param>
    [RelayCommand]
    private async Task Delete(SaveItemModel model)
    {
        IsBusy = true;
        if (model.SendType==SendType.File)
        {
            if (File.Exists(model.Content))
            {
                File.Delete(model.Content);
            }
        }

        FantasyResultModel.ResultBase<bool> res = await _saveDataService.DeleteDataAsync(model.Guid);
        if (res.Ok)
        {
            Models.Remove(model);
        }
        else
        {
            await _dialogService.DisplayAlert("Warning", res.ErrorMsg, "Ok");
        }
        IsBusy = false;
    }


    [RelayCommand]
    public Task OpenFolder(SaveItemModel model)
    {
        if (string.IsNullOrEmpty(model.Content))
        {
            return _dialogService.DisplayAlert("警告", "文件路径不存在", "确定");
        }

        
        var p = model.SendType==SendType.File ? Directory.GetParent(model.Content)?.ToString() : model.Content;
        if (string.IsNullOrEmpty(p))
            return _dialogService.DisplayAlert("警告", "文件夹父路径不存在", "确定");

        _openFolder.OpenFolder(p);
        return Task.CompletedTask;
    }


    [RelayCommand]
    private async Task Detail(SaveItemModel model)
    {
        NavigationParameter parameter = new NavigationParameter();
        parameter.Add("data", model.Content);
        await _navigationService.NavigationToAsync(nameof(DetailPage), parameter);
    }

    [RelayCommand]
    public async Task Init()
    {
        try
        {
            IsBusy = true;
            FantasyResultModel.ResultBase<List<SaveDataModel>> list = await _saveDataService.GetAllAsync();
            if (list.Ok)
            {
                list.Data.Reverse();
                rig(list.Data);
            }
            else
            {
                await _dialogService.DisplayAlert("Warning", list.ErrorMsg, "Ok");
            }
        }
        finally
        {
            IsBusy = false;
        }
    }
}