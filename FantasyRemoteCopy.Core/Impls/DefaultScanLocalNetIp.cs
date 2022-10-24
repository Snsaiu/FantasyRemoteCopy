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
           var localIps = localIpResult.Data;

           List<string> ips = new List<string>();
          
           List<Task> tasks=new List<Task>();

           foreach (var ip in localIps)
           {

              var t=  Task.Run(() =>
               {
                   string ipDuan = ip.Remove(ip.LastIndexOf('.'));
                   //MessageBox.Show(ipDuan);
                   //枚举网段计算机
                   Ping myPing = new Ping();
                   string data = "";
                   byte[] buffer = Encoding.ASCII.GetBytes(data);


                   for (int i = 1; i <= 255; i++)
                   {
                       string pingIP = ipDuan + "." + i.ToString();

                       try
                       {
                           PingReply pingReply = myPing.Send(pingIP, 50, buffer);

                           if (pingReply.Status == IPStatus.Success)
                           {

                               ips.Add(pingIP);
                           }
                       }
                       catch (Exception e)
                       {

                       }
                   }

               });
          
                tasks.Add(t);
  
           }

           await Task.WhenAll(tasks);
            for (int i = 0; i < localIps.Count; i++)
            {
                for (int j = 0; j < ips.Count; j++)
                {
                    if (ips[j] == localIps[i])
                    {
                        ips.RemoveAt(j);
                        j--;
                    }
                }
            }
            return new SuccessResultModel<List<string>>(ips);

       }
       catch (Exception e)
       {
           return new ErrorResultModel<List<string>>(e.Message);
       }
      
    }
}