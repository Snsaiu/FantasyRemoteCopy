using AirTransfer.Interfaces;
using FantasyRemoteCopy.UI.Interfaces;

namespace AirTransfer.Models;

public class PortCheckResultModel : IFlag
{
    public string Port { get; }

    public PortCheckResultModel(string port, bool canUse, string flag)
    {
        Port = port;
        CanUse = canUse;
        Flag = flag;
    }

    public bool CanUse { get; }
    public string Flag { get; }
}