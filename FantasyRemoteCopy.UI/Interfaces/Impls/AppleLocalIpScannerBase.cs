using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Models;
using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class AppleLocalIpScannerBase:IGetLocalNetDevices
{
    
    public async IAsyncEnumerable<ScanDevice> GetDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var scanIps = await Task.Run(() =>
        {
            var temp = new List<string>();
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
                //res.Add(m.Groups["ip"].Value);
                temp.Add(m.Groups["ip"].Value);
            }

            return temp.AsReadOnly();
        }, cancellationToken);

        if (!scanIps.Any())
            yield break;
        foreach (string item in scanIps)
        {
            if (item.StartsWith("192.168"))
                yield return new ScanDevice(SystemType.MacOS,Device.Desktop, item,"name");
        }
        
    }
}