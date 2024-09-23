using System.Net.NetworkInformation;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces;

public class GlobalScanBase : ISendeable<string>
{
    public async Task SendAsync(string message)
    {
        string ipDuan = message.Remove(message.LastIndexOf('.'));

        string data = String.Empty;
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
                finally
                {

                }
            }
        });

    }
}