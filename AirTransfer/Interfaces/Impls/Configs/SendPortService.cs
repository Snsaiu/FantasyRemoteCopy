using AirTransfer.Interfaces;

namespace FantasyRemoteCopy.UI.Interfaces.Impls
{
    public class SendPortService : ISendPortService
    {
        public int GetPort()
        {
            return Preferences.Default.Get<int>("port", 5678);
        }

        public void SetPort(int port)
        {
            Preferences.Default.Set<int>("port", port);
        }
    }
}