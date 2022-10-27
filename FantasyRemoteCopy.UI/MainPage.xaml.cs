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

using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
	private readonly SendDataBussiness _sendDataBussiness;
	private readonly ReceiveBussiness _receiveBussiness;
    private readonly IUserService userService;


	private List<SendInviteModel> sendInviteModels= new List<SendInviteModel>();

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
            this.sendInviteModels.Add(model);
            Application.Current.Dispatcher.Dispatch(() =>
			{
				
				this.info.Text += $"{model.DeviceIP} {model.DeviceName} {model.DevicePlatform.ToString()}" + "\n";
			});
		
        };

	
    }
   

    private  void OnCounterClicked(object sender, EventArgs e)
    {
		this.sendInviteModels.Clear();
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

		tf.TargetIp = this.sendInviteModels.First().DeviceIP;

		string sendTxt = "这是一个测试数据";
		byte[] bytes= Encoding.UTF8.GetBytes(sendTxt);
		DataMetaModel dm=new DataMetaModel { Guid= tf.DataGuid ,Size=bytes.Length, Sended=false};
		
		tf.Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dm));


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


