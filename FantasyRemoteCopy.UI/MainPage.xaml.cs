using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.Core;
using FantasyRemoteCopy.Core.Impls;

namespace FantasyRemoteCopy.UI;

public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}
	

	private async void OnCounterClicked(object sender, EventArgs e)
    {


        this.info.Text = DateTime.Now.ToString();

        IScanLocalNetIp scanLocalNetIp = new DefaultScanLocalNetIp(new DefaultLocalIp());
	  var res=	await scanLocalNetIp.ScanLocalNetIpAsync();
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


