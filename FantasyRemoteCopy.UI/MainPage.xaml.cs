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


	public MainPage(SendDataBussiness sendDataBussiness,
		ReceiveBussiness receiveBussiness)
	{
		_sendDataBussiness = sendDataBussiness;
		_receiveBussiness = receiveBussiness;

		InitializeComponent();
		this.info.Text = "";
		this.indicator.IsVisible = false;
        this._receiveBussiness.DiscoverEnableIpEvent += (ip) =>
        {
			Application.Current.Dispatcher.Dispatch(() =>
			{
				this.info.Text += ip + "\n";
			});
		
        };
    }
   

    private  void OnCounterClicked(object sender, EventArgs e)
    {
		this.info.Text = "";

		this.indicator.IsVisible = true;
		 Task.Run(async() =>
		{
            await this._sendDataBussiness.DeviceDiscover();
		}).WaitAsync(TimeSpan.FromSeconds(10)).GetAwaiter().OnCompleted(() =>
		{
			this.indicator.IsVisible = false;
		});

		   

	  
        
		

    }

    private void AddUserEvent(object sender, EventArgs e)
    {
        // this._userService.SaveUser(new UserInfo
        // {
        //     Name = "小明"
        // });

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


