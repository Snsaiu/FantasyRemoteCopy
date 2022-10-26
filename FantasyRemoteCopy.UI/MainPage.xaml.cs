using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Models;
using FantasyRemoteCopy.UI.Bussiness;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
	private readonly SendDataBussiness _sendDataBussiness;
	private readonly ReceiveBussiness _receiveBussiness;
    private readonly IUserService userService;

    public MainPage(SendDataBussiness sendDataBussiness,
		ReceiveBussiness receiveBussiness,IUserService userService)
	{
		_sendDataBussiness = sendDataBussiness;
		_receiveBussiness = receiveBussiness;
        this.userService = userService;
		

        InitializeComponent();
		this.info.Text = "";
		this.indicator.IsVisible = false;
        this._receiveBussiness.DiscoverEnableIpEvent += (model) =>
        {
			Application.Current.Dispatcher.Dispatch(() =>
			{
				this.info.Text += $"{model.MasterName} {model.DeviceName} {model.DevicePlatform.ToString()}" + "\n";
			});
		
        };
    }
   

    private  void OnCounterClicked(object sender, EventArgs e)
    {
		this.info.Text = "";
		var userRes= this.userService.GetCurrentUser().GetAwaiter().GetResult();
		if(userRes.Ok==false)
		{
			this.userService.SaveUser(new UserInfo { Name = "小明" });
		}
		this.indicator.IsVisible = true;
		 Task.Run(async() =>
		{
            await this._sendDataBussiness.DeviceDiscover();
		}).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(() =>
		{
			this.indicator.IsVisible = false;
		});

    }

    private async void AddUserEvent(object sender, EventArgs e)
    {
        var tf = new TransformData { Type = DataType.BuildConnected };
        tf.DataGuid = "479237";

        tf.TargetIp = "192.168.0.106";
		


        await this._sendDataBussiness.SendData(tf);

    }

    private async void GetUserEvent(object sender, EventArgs e)
    {
	  // var res=  await this._userService.GetCurrentUser();
	  // if (res.Ok)
	  // {
		 //  this.info.Text = "找到用户：" + res.Data.Name;
	  // }
    }
}


