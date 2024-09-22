using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Models;

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class LocalIpScannerBase : IGetLocalNetDevices
{
    protected virtual string Pattern { get; }=@"(?<ip>([0-9]{1,3}\.?){4})\s*\) at ([0-9]|[a-z])";
    
    public async IAsyncEnumerable<ScanDevice> GetDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        System.Collections.ObjectModel.ReadOnlyCollection<string> scanIps = await Task.Run(() =>
        {
            List<string> temp = [];
            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "arp";
            pProcess.StartInfo.Arguments = "-a ";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;
            pProcess.Start();
            string cmdOutput = pProcess.StandardOutput.ReadToEnd();
         
            foreach (Match m in Regex.Matches(cmdOutput, Pattern, RegexOptions.IgnoreCase))
            {
                temp.Add(m.Groups["ip"].Value);
            }

            return temp.AsReadOnly();
        }, cancellationToken);

        if (!scanIps.Any())
            yield break;
        foreach (string item in scanIps)
        {
            if (item.StartsWith("192.168"))
                yield return new ScanDevice(SystemType.None, Device.None,item, null);
        }

    }
}