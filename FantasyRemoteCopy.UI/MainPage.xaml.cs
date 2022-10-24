using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Impls;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
    private readonly IScanLocalNetIp scanLocalNetIp;
    int count = 0;

	public MainPage(IScanLocalNetIp scanLocalNetIp)
	{
		InitializeComponent();
        this.scanLocalNetIp = scanLocalNetIp;
    }
   

    private async void OnCounterClicked(object sender, EventArgs e)
    {


        var res= await this.scanLocalNetIp.ScanLocalNetIpAsync();
  
	  //var res=	await scanLocalNetIp.ScanLocalNetIpAsync();
	  if (res.Ok)
	  {
		 string x=  string.Join("*", res.Data);
		 this.info.Text = x;
	  }
	  else
	  {
		  this.info.Text = res.ErrorMsg;
	  }


	}
}


