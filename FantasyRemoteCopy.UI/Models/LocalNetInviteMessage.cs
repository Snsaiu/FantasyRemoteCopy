namespace FantasyRemoteCopy.UI.Models;

public class LocalNetInviteMessage(string name,string ip) : IDeviceName
{
    public string Ip { get; } = ip;
    
    public string Name { get; } = name;
}

