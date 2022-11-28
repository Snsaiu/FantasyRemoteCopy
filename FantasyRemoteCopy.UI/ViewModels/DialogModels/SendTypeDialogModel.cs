using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;
using FantasyMvvm.FantasyModels.Impls;
using FantasyMvvm.FantasyNavigation;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Models;
using FantasyRemoteCopy.UI.Views;

namespace FantasyRemoteCopy.UI.ViewModels.DialogModels;

public partial class SendTypeDialogModel : FantasyDialogModelBase
{
    public override event OnCloseDelegate OnCloseEvent;
    private DiscoveredDeviceModel discoveredDeviceModel;

    private readonly INavigationService _navigationService;

    private readonly SendDataBussiness _sendDataBussiness;

    private readonly IDialogService _dialogService;

    [ObservableProperty]
    private bool isBusy = false;

    public SendTypeDialogModel(INavigationService navigationService, SendDataBussiness sendData,IDialogService dialogService)
    {
        this._dialogService = dialogService;
        this._sendDataBussiness = sendData;
        this._navigationService = navigationService;
    }

    public override void OnParameter(INavigationParameter parameter)
    {
      this.discoveredDeviceModel=parameter.Get<DiscoveredDeviceModel>("data");
    }

    [ICommand]
    public async void TextInput()
    {
        NavigationParameter parameter = new NavigationParameter();
        parameter.Add("data", discoveredDeviceModel);

        await this._navigationService.NavigationToAsync(nameof(TextInputPage), parameter);
    }

    public async void FileInput()
    {
        try
        {
          

            var f = await FilePicker.PickAsync();

            if (f != null)
            {

                FileInfo finfo = new FileInfo(f.FullPath);
                if (finfo.Length > 1073741824)
                {
                    
                    await App.Current.MainPage.DisplayAlert("Warning", "File cannot be larger than 1G ！", "Ok");
                    return;
                }
                this.IsBusy = true;
                var res = await this._sendDataBussiness.SendData(this.discoveredDeviceModel.Ip, f.FullPath, DataType.File);

                if (res != null)
                {
                    this.IsBusy = false;
#if WINDOWS
                  await Application.Current.MainPage.DisplayAlert("Information", "Sended!", "Ok");
#endif


                    this.OnCloseEvent(new CloseResultModel { Success = true });

                }



            }
            this.OnCloseEvent(new CloseResultModel { Success = false });

        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw;
        }

    }
}