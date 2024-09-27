using System.Net.NetworkInformation;
using System.Text;

namespace FantasyRemoteCopy.UI.Interfaces.Impls;

public class GlobalScanBase : ISendeable<string>
{
    public async Task SendAsync(string message, CancellationToken cancellationToken)
    {
        string ipDuan = message.Remove(message.LastIndexOf('.'));

        string data = string.Empty;
        byte[] buffer = Encoding.ASCII.GetBytes(data);

        await Task.Run(() =>
        {
            for (int i = 1; i < 255; i++)
            {
                string pingIP = ipDuan + "." + i.ToString();


                cancellationToken.ThrowIfCancellationRequested();

                Ping myPing = new Ping();

                myPing.SendAsync(pingIP, 20, cancellationToken);

            }
        });

    }
}