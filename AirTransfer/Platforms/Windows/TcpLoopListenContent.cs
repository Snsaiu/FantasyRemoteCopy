using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.TcpTransfer;
using AirTransfer.Interfaces.Impls.Configs;
namespace AirTransfer;


public sealed class TcpLoopListenContent(FileSavePathBase fileSavePathBase) :TcpLoopListenContentBase(fileSavePathBase)
{

}