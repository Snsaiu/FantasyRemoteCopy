using FantasyResultModel;
using FantasyResultModel.Impls;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Media;

using System.Net.NetworkInformation;

using System.Text;

namespace FantasyRemoteCopy.Core.Platforms;

public class GlobalScanLocalNetIp:IGlobalScanLocalNetIp
{
    private readonly IGetLocalIp _getLocalIp;
    public GlobalScanLocalNetIp(IGetLocalIp getLocalIp)
    {
        _getLocalIp = getLocalIp;
    }

    public async Task<ResultBase<bool>> GlobalSearch()
    {

        var localIpResult = this._getLocalIp.GetLocalIp();
        if (localIpResult.Ok == false)
            return await Task.FromResult(new ErrorResultModel<bool>(localIpResult.ErrorMsg));

        try
        {
            List<string> localIps = localIpResult.Data;

            foreach (var ip in localIps)
            {
                string ipDuan = ip.Remove(ip.LastIndexOf('.'));
                //MessageBox.Show(ipDuan);
                //枚举网段计算机
               
                string data = "";
                byte[] buffer = Encoding.ASCII.GetBytes(data);

                await Task.Run(() =>
                {
                    for (int i = 1; i < 255; i++)
                    {
                        string pingIP = ipDuan + "." + i.ToString();

                        try
                        {
                            Ping myPing = new Ping();

                            PingReply pingReply = myPing.Send(pingIP, 20, buffer);

                           
                        }
                        catch (Exception e)
                        {
                        }


                    }


                });
            }

           

            return new SuccessResultModel<bool>(true);

        }
        catch (Exception e)
        {
            return new ErrorResultModel<bool>(e.Message);
        }


    }
}