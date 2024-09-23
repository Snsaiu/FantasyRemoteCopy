using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Models;

using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using Device = FantasyRemoteCopy.Core.Enums.Device;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class LocalIpScannerBase : IGetLocalNetDevices
{
    private readonly DeviceLocalIpBase _deviceLocalIpBase;
    protected virtual string Pattern { get; } = @"(?<ip>([0-9]{1,3}\.?){4})\s*\) at ([0-9]|[a-z])";

    public LocalIpScannerBase(DeviceLocalIpBase deviceLocalIpBase)
    {
        _deviceLocalIpBase = deviceLocalIpBase;
    }

    public async IAsyncEnumerable<ScanDevice> GetDevicesAsync([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        string localIp = await _deviceLocalIpBase.GetLocalIpAsync();

        List<string> list = localIp.Split(".").ToList()[0..2];
        string ipStart = string.Join(".", list);

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
            if (item.StartsWith(ipStart))
                yield return new ScanDevice(SystemType.None, Device.None, item, null);
        }

    }


    private static IPAddress GetLowestIp(IPAddress address, IPAddress mask)
    {
        byte[] addressBytes = address.GetAddressBytes();
        byte[] maskBytes = mask.GetAddressBytes();
        if (addressBytes.Length != 4 || maskBytes.Length != 4)
            return IPAddress.None;
        byte[] lowest = new byte[4];
        for (int i = 0; i < 4; i++)
            lowest[i] = (byte)(addressBytes[i] & maskBytes[i]);
        return new IPAddress(lowest);
    }

    private static IPAddress GetHighestIp(IPAddress address, IPAddress mask)
    {
        byte[] addressBytes = address.GetAddressBytes();
        byte[] maskBytes = mask.GetAddressBytes();
        if (addressBytes.Length != 4 || maskBytes.Length != 4)
            return IPAddress.None;
        byte[] highest = new byte[4];
        for (int i = 0; i < 4; i++)
            highest[i] = (byte)((addressBytes[i] & maskBytes[i]) | ~maskBytes[i]);
        return new IPAddress(highest);
    }

}
