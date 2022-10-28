using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;

using System.Collections.ObjectModel;

namespace FantasyRemoteCopy.UI.ViewModels;


[ObservableObject]
public partial class ListPageModel
{
    private readonly ISaveDataService _saveDataService;


    [ObservableProperty]
    private ObservableCollection<SaveItemModel> models = new ObservableCollection<SaveItemModel>();


    public ListPageModel(ISaveDataService saveDataService)
    {
        _saveDataService = saveDataService;
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
                    sm.Title=item.Content.Substring(0,20)+"...";
                }
                else
                {
                    sm.Title = item.Content;
                }
                sm.IsText = true;
                sm.IsFile = false;
                sm.Image = ImageSource.FromFile("texticon.png");
            }
            else
            {
                sm.IsText = false;
                sm.IsFile = true;
                sm.Title = item.Content;
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
    public async void Init()
    {
       var list= await  this._saveDataService.GetAllAsync();
       if (list.Ok)
       {
           this.rig(list.Data);
       }
       else
       {
         await  App.Current.MainPage.DisplayAlert("Warning", list.ErrorMsg, "Ok");

       }
    }
}