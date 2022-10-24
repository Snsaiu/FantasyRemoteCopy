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
    public void GetAllMacAddressesAndIppairs()
    {
      
        System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
        pProcess.StartInfo.FileName = "arp";
        pProcess.StartInfo.Arguments = "-a ";
        pProcess.StartInfo.UseShellExecute = false;
        pProcess.StartInfo.RedirectStandardOutput = true;
        pProcess.StartInfo.CreateNoWindow = true;
        pProcess.Start();
        string cmdOutput = pProcess.StandardOutput.ReadToEnd();
        string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";

        foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
        {

            this.info.Text += m.Groups["ip"].Value+"\n";
         
        }

       
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


