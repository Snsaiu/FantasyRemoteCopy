using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Bussiness;
using FantasyRemoteCopy.Core.Models;

using Newtonsoft.Json;

using System.Text;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
    private readonly SendDataBussiness _sendDataBussiness;
    private readonly ReceiveBussiness _receiveBussiness;
    private readonly IUserService userService;


    private List<SendInviteModel> sendInviteModels = [];

    public MainPage(SendDataBussiness sendDataBussiness,
        ReceiveBussiness receiveBussiness, IUserService userService)
    {
        _sendDataBussiness = sendDataBussiness;
        _receiveBussiness = receiveBussiness;
        this.userService = userService;


        InitializeComponent();
        info.Text = "";
        indicator.IsVisible = false;
        _receiveBussiness.DiscoverEnableIpEvent += (model) =>
        {
            sendInviteModels.Add(model);
            Application.Current.Dispatcher.Dispatch(() =>
            {

                info.Text += $"{model.DeviceIP} {model.DeviceName} {model.DevicePlatform}" + "\n";
            });

        };

        _receiveBussiness.ReceiveDataEvent += (d) =>
        {
            string str = Encoding.UTF8.GetString(d.Data);
            DataMetaModel dm = JsonConvert.DeserializeObject<DataMetaModel>(str);


        };


    }


    private void OnCounterClicked(object sender, EventArgs e)
    {
        sendInviteModels.Clear();
        info.Text = "";
        FantasyResultModel.ResultBase<UserInfo> userRes = userService.GetCurrentUserAsync().GetAwaiter().GetResult();
        if (userRes.Ok == false)
        {
            userService.SaveUserAsync(new UserInfo { Name = "小明" });
        }
        indicator.IsVisible = true;
        Task.Run(async () =>
       {
           await _sendDataBussiness.DeviceDiscover();
       }).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(() =>
       {
           indicator.IsVisible = false;
       });

    }

    private async void AddUserEvent(object sender, EventArgs e)
    {


        await _sendDataBussiness.SendData(sendInviteModels.First().DeviceIP, "helloworld", DataType.Text);

    }

    private void GetUserEvent(object sender, EventArgs e)
    {
        // var res=  await this._userService.GetCurrentUser();
        // if (res.Ok)
        // {
        //  this.info.Text = "找到用户：" + res.Data.Name;
        // }
    }
}


