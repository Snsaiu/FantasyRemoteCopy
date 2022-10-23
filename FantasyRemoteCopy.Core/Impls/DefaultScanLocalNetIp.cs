using System.Net.NetworkInformation;
using System.Text;
using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Impls;

public class DefaultScanLocalNetIp:IScanLocalNetIp
{
    private readonly IGetLocalIp _getLocalIp;

    public DefaultScanLocalNetIp(IGetLocalIp getLocalIp)
    {
        _getLocalIp = getLocalIp;
    }
    
    public async Task<ResultBase<List<string>>> ScanLocalNetIpAsync()
    {
       var localIpResult=  this._getLocalIp.GetLocalIp();
       if (localIpResult.Ok == false)
           return await Task.FromResult(new ErrorResultModel<List<string>>(localIpResult.ErrorMsg));

       try
       {
           string localIp = localIpResult.Data;

           List<string> ips = new List<string>();

           string ipDuan = localIp.Remove(localIp.LastIndexOf('.'));
           //MessageBox.Show(ipDuan);
           //枚举网段计算机
           Ping myPing = new Ping();
           string data = "";
           byte[] buffer = Encoding.ASCII.GetBytes(data);

             await Task.Run(() =>
           {
               for (int i = 1; i <= 255; i++)
               {
                   string pingIP = ipDuan + "." + i.ToString();

                   try
                   {
                       PingReply pingReply = myPing.Send(pingIP, 30, buffer);

                       if (pingReply.Status == IPStatus.Success)
                       {
                
                           ips.Add(pingIP);
                       }
                   }
                   catch (Exception e)
                   {
                    
                   }
                 
  
               }

               for (int i = 0; i < ips.Count; i++)
               {
                   if(ips[i]==localIp)
                       ips.RemoveAt(i);
               }

           });
           
           return new SuccessResultModel<List<string>>(ips);

       }
       catch (Exception e)
       {
           return new ErrorResultModel<List<string>>(e.Message);
       }
      
    }
}