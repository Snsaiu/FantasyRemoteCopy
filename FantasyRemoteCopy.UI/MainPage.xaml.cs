using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.Core.Impls;
using FantasyRemoteCopy.Core.Models;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
    private readonly IScanLocalNetIp scanLocalNetIp;
    private readonly ISendData _sendData;
    private readonly IReceiveData _receiveData;
    private readonly IUserService _userService;
    int count = 0;

	public MainPage(IScanLocalNetIp scanLocalNetIp
		,ISendData sendData
		,IReceiveData receiveData
		,IUserService userService)
	{
		InitializeComponent();
        this.scanLocalNetIp = scanLocalNetIp;
        _sendData = sendData;
        _receiveData = receiveData;
        _userService = userService;
        this._receiveData.LiseningInvite();
	}
   

    private async void OnCounterClicked(object sender, EventArgs e)
    {

	    TransformData td = new TransformData();
	    td.Port = "5976";
	    td.TargetIp = "192.168.0.106";
	    td.Type = DataType.ValidateAccount;
	    string txt = "hello world";
		td.Data= Encoding.UTF8.GetBytes(txt);

		await this._sendData.SendInviteAsync(td);
	    //      var res= await this.scanLocalNetIp.ScanLocalNetIpAsync();
	    //
	    // //var res=	await scanLocalNetIp.ScanLocalNetIpAsync();
	    // if (res.Ok)
	    // {
	    // string x=  string.Join("*", res.Data);
	    // this.info.Text = x;
	    // }
	    // else
	    // {
	    //  this.info.Text = res.ErrorMsg;
	    // }


    }

    private void AddUserEvent(object sender, EventArgs e)
    {
        this._userService.SaveUser(new UserInfo
        {
            Name = "小明"
        });

    }

    private async void GetUserEvent(object sender, EventArgs e)
    {
	  var res=  await this._userService.GetCurrentUser();
	  if (res.Ok)
	  {
		  this.info.Text = "找到用户：" + res.Data.Name;
	  }
    }
}


