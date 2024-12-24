using AirTransfer.Interfaces.Impls.Configs;
using AirTransfer.Interfaces.Impls.TcpTransfer;

namespace AirTransfer;

public sealed class TcpLoopListenContent(FileSavePathBase fileSavePathBase) : TcpLoopListenContentBase(fileSavePathBase)
{
}