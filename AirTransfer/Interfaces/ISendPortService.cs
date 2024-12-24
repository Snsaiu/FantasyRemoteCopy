namespace AirTransfer.Interfaces;

public interface ISendPortService
{
    int GetPort();
    
    void SetPort(int port);
}