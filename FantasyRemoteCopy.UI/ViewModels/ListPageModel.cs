using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;

using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

using System.Collections.ObjectModel;
using FantasyRemoteCopy.Core.Enums;
using SaveDataModel = FantasyRemoteCopy.UI.Models.SaveDataModel;
using SaveDataType = FantasyRemoteCopy.UI.Models.SaveDataType;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class ListPageModel : FantasyPageModelBase
{
    private readonly ISaveDataService _saveDataService;
    private readonly IOpenFolder _openFolder;



    [ObservableProperty]
    private ObservableCollection<SaveItemModel> models = [];

    [ObservableProperty]
    private bool isBusy = false;


    private readonly IDialogService? _dialogService = null;

    private readonly INavigationService? _navigationService = null;

    public ListPageModel(ISaveDataService saveDataService, IOpenFolder openFolder, IDialogService dialogService, INavigationService navigationService)
    {
        _saveDataService = saveDataService;
        _openFolder = openFolder;
        _dialogService = dialogService;
        _navigationService = navigationService;
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
                sm.IsText = true;
                sm.IsFile = false;
                sm.Image = ImageSource.FromFile("texticon.png");
            }
            else
            {
                sm.IsText = false;
                sm.IsFile = true;

                sm.Title = Path.GetFileName(item.Content);
                sm.Image = ImageSource.FromFile("fileicon.png");
            }

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
        // 判断文件是否存在
        if (File.Exists(model.Content) == false)
        {
            FantasyResultModel.ResultBase<bool> res = await _saveDataService.DeleteDataAsync(model.Guid);
            if (res.Ok)
            {
                Models.Remove(model);
            }
            else
            {
                await _dialogService.DisplayAlert("Warning", res.ErrorMsg, "Ok");
            }
            return;
        }
        IsBusy = true;

#if WINDOWS
                var openOk=  await Launcher.OpenAsync(model.Content);
          if (openOk)
          {
          }
          else
          {
              await this._dialogService.DisplayAlert("Warning", $"{model.Title} Open Error", "Ok");
        }  
#elif MACCATALYST

        System.Diagnostics.Process.Start("open", model.Content);

#endif

        IsBusy = false;
    }

    /// <summary>
    /// 删除命令
    /// </summary>
    /// <param name="model"></param>
    [RelayCommand]
    private async Task Delete(SaveItemModel model)
    {
        IsBusy = true;
        if (model.IsFile)
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
    public void OpenFolder(SaveItemModel model)
    {
        string p = Directory.GetParent(model.Content).ToString();
        //var p=  Path.GetFullPath(model.Content);

        _openFolder.OpenFolder(p);
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
        IsBusy = false;
    }
}