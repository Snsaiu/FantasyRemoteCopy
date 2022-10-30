using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;

using System.Collections.ObjectModel;
using FantasyRemoteCopy.UI.Views;
using Windows.Storage.Pickers;

namespace FantasyRemoteCopy.UI.ViewModels;


[ObservableObject]
public partial class ListPageModel
{
    private readonly ISaveDataService _saveDataService;
    private readonly IOpenFolder _openFolder;
    private readonly FolderPicker _folderPicker;


    [ObservableProperty]
    private ObservableCollection<SaveItemModel> models = new ObservableCollection<SaveItemModel>();

    [ObservableProperty]
    private bool isBusy = false;



    public ListPageModel(ISaveDataService saveDataService,IOpenFolder openFolder)
    {
        _saveDataService = saveDataService;
        _openFolder = openFolder;
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
       await Application.Current.MainPage.DisplayAlert("Information", "Success copy!", "Ok");


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
                await Application.Current.MainPage.DisplayAlert("Warning", res.ErrorMsg, "Ok");
            }
            return;
        }
        this.IsBusy = true;
       
          var openOk=  await Launcher.OpenAsync(model.Content);
          if (openOk)
          {
              //var t= Toast.Make($"{model.Title}已经打开",CommunityToolkit.Maui.Core.ToastDuration.Short,15);
              //await t.Show();
            // await Application.Current.MainPage.DisplaySnackbar("hello");
              //await Application.Current.MainPage.DisplayAlert("Information", $"{model.Title}已经打开", "Ok");
          }
          else
          {
              await Application.Current.MainPage.DisplayAlert("Warning", $"{model.Title}打开失败", "Ok");
        }
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
       var res=  await this._saveDataService.DeleteDataAsync(model.Guid);
       if (res.Ok)
       {
           this.Models.Remove(model);
       }
       else
       {
           await Application.Current.MainPage.DisplayAlert("Warning", res.ErrorMsg, "Ok");
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
       await Application.Current.MainPage.Navigation.PushAsync(new DetailPage(model.Content));
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
         await  App.Current.MainPage.DisplayAlert("Warning", list.ErrorMsg, "Ok");

       }
       this.IsBusy = false;
    }
}