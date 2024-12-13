using AirTransfer.Enums;
using AirTransfer.Interfaces;


namespace AirTransfer.Models;

public class TransformResultModel<T>(string flag, SendType sendType, T result, int port) : IFlag, ISendType, IPort
{
    public T Result { get; init; } = result;

    public string Flag { get; init; } = flag;
    public SendType SendType { get; init; } = sendType;
    public int Port { get; init; } = port;
}