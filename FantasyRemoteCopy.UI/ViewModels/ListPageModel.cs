using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;

using System.Collections.ObjectModel;
using FantasyRemoteCopy.UI.Views;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyNavigation;
using FantasyMvvm.FantasyModels.Impls;

namespace FantasyRemoteCopy.UI.ViewModels;

public partial class ListPageModel:FantasyPageModelBase
{
    private readonly ISaveDataService _saveDataService;
    private readonly IOpenFolder _openFolder;



    [ObservableProperty]
    private ObservableCollection<SaveItemModel> models = new ObservableCollection<SaveItemModel>();

    [ObservableProperty]
    private bool isBusy = false;


    private readonly IDialogService _dialogService = null;

    private readonly INavigationService _navigationService = null;

    public ListPageModel(ISaveDataService saveDataService,IOpenFolder openFolder,IDialogService dialogService,INavigationService navigationService)
    {
        _saveDataService = saveDataService;
        _openFolder = openFolder;
        this._dialogService = dialogService;
        this._navigationService = navigationService;
    }


    private void rig(List<SaveDataModel> sources)
    {
        foreach (var item in sources)
        {
            SaveItemModel sm = new SaveItemModel();
            if (item.DataType == SaveDataType.Txt)
            {
                if (item.Content.Length > 20)
                {
                    sm.Title=item.Content.Replace(" ","").Replace("\n","").Substring(0,20)+"...";
                }
                else
                {
                    sm.Title = item.Content.Replace(" ", "").Replace("\n", "");
                }
                sm.IsText = true;
                sm.IsFile = false;
                sm.Image = ImageSource.FromFile("texticon.png");
            }
            else
            {
                sm.IsText = false;
                sm.IsFile = true;

                sm.Title = Path.GetFileName( item.Content);
                sm.Image = ImageSource.FromFile("fileicon.png");
            }

            sm.Content = item.Content;
            sm.Guid= item.Guid;
            sm.SourceDeviceName = item.SourceDeviceNickName;
            sm.Time=item.Time.ToString("yyyy-MM-dd HH:mm:ss");

            this.Models.Add(sm);
        }
    }


    [ICommand]
    public async void CopyContent(SaveItemModel model)
    {
       await Clipboard.Default.SetTextAsync(model.Content);
       await this._dialogService.DisplayAlert("Information", "Success copy!", "Ok");
       


    }


    [ICommand]
    public async void OpenFile(SaveItemModel model)
    {
        // 判断文件是否存在
        if (File.Exists(model.Content) == false)
        {
            var res = await this._saveDataService.DeleteDataAsync(model.Guid);
            if (res.Ok)
            {
                this.Models.Remove(model);
            }
            else
            {
                await this._dialogService.DisplayAlert("Warning", res.ErrorMsg, "Ok");
            }
            return;
        }
        this.IsBusy = true;

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

            this.IsBusy = false;
            
       


    }

    /// <summary>
    /// 删除命令
    /// </summary>
    /// <param name="model"></param>
    [ICommand]
    public async void Delete(SaveItemModel model)
    {
        this.IsBusy = true;
        if (model.IsFile)
        {
            if (File.Exists(model.Content))
            {
                File.Delete(model.Content);
            }
        }
        
       var res=  await this._saveDataService.DeleteDataAsync(model.Guid);
       if (res.Ok)
       {
           this.Models.Remove(model);
       }
       else
       {
           await this._dialogService.DisplayAlert("Warning", res.ErrorMsg, "Ok");
        }
       this.IsBusy = false;
    }


    [ICommand]
    public async void OpenFolder(SaveItemModel model)
    {
        var p= Directory.GetParent(model.Content).ToString();
       //var p=  Path.GetFullPath(model.Content);

       this._openFolder.OpenFolder(p);
    }


    [ICommand]
    public async void Detail(SaveItemModel model)
    {
 
        NavigationParameter parameter = new NavigationParameter();
        parameter.Add("data", model.Content);
        await this._navigationService.NavigationToAsync(nameof(DetailPage), parameter);
    }

    [ICommand]
    public async void Init()
    {
        this.IsBusy = true;
       var list= await  this._saveDataService.GetAllAsync();
       if (list.Ok)
       {
           list.Data.Reverse();
           this.rig(list.Data);
       }
       else
       {
         await  this._dialogService.DisplayAlert("Warning", list.ErrorMsg, "Ok");

       }
       this.IsBusy = false;
    }
}