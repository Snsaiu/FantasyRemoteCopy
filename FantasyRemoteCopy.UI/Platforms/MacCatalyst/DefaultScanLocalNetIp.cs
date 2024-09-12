using FantasyRemoteCopy.UI.Interfaces;

using FantasyResultModel;
using FantasyResultModel.Impls;

using System.Text.RegularExpressions;

namespace FantasyRemoteCopy.UI;

public class DefaultScanLocalNetIp : IScanLocalNetIp
{
    private readonly IGetLocalIp _getLocalIp;
    public DefaultScanLocalNetIp(IGetLocalIp getLocalIp)
    {
        _getLocalIp = getLocalIp;
    }

    public async Task<ResultBase<List<string>>> ScanLocalNetIpAsync()
    {
        List<string> res = [];

        ResultBase<List<string>> localIpResult = _getLocalIp.GetLocalIp();
        if (localIpResult.Ok == false)
            return await Task.FromResult(new ErrorResultModel<List<string>>(localIpResult.ErrorMsg));

        try
        {
            await Task.Run(() =>
            {
                System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
                pProcess.StartInfo.FileName = "arp";
                pProcess.StartInfo.Arguments = "-a ";
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                string cmdOutput = pProcess.StandardOutput.ReadToEnd();
                string pattern = @"(?<ip>([0-9]{1,3}\.?){4})\s*\) at ([0-9]|[a-z])";

                foreach (Match m in Regex.Matches(cmdOutput, pattern, RegexOptions.IgnoreCase))
                {
                    res.Add(m.Groups["ip"].Value);
                }

            });

            List<string> ipsres = [];
            for (int i = 0; i < localIpResult.Data.Count; i++)
            {
                string ipDuan = localIpResult.Data[i].Remove(localIpResult.Data[i].LastIndexOf('.'));

                for (int j = 0; j < res.Count; j++)
                {
                    if (localIpResult.Data[i] == res[j])
                    {
                        res.RemoveAt(j);
                        j--;
                        continue;
                    }

                    if (res[j].EndsWith(".255"))
                    {
                        res.RemoveAt(j);
                        j--;
                        continue;
                    }

                    if (res[j].StartsWith(ipDuan))
                    {
                        ipsres.Add(res[j]);
                    }
                }

            }

            return new SuccessResultModel<List<string>>(ipsres);


        }
        catch (Exception e)
        {
            return new ErrorResultModel<List<string>>(e.Message);
        }



    }
}

