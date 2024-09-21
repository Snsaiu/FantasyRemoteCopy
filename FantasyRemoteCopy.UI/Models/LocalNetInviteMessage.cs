using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Models;

public class LocalNetInviteMessage : IName
{
    [JsonConstructor]
    public LocalNetInviteMessage()
    {
        
    }
    public LocalNetInviteMessage(string name,string ip)
    {
        Ip = ip;
        Name = name;
    }

    public string Ip { get; }
    
    public string Name { get; }
}

