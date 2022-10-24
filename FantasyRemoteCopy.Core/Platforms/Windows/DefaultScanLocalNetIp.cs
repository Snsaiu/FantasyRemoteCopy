using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using FantasyResultModel;
using FantasyResultModel.Impls;

namespace FantasyRemoteCopy.Core.Platforms
{
    public class DefaultScanLocalNetIp : IScanLocalNetIp
    {
        public DefaultScanLocalNetIp()
        {
        }

        public async Task<ResultBase<List<string>>> ScanLocalNetIpAsync()
        {
            List<string> res = new List<string>();
            try
            {
              await  Task.Run(() =>
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
                        res.Add(m.Groups["ip"].Value);
                    }

                });

                return new SuccessResultModel<List<string>>(res);


            }
            catch (Exception e)
            {
                return new ErrorResultModel<List<string>>(e.Message);
            }

       

        }
    }
}
