using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls;
using AirTransfer.Interfaces.Impls.Configs;
namespace AirTransfer;


public sealed class FileSavePath(ISavePathService savePathService) : DeskTopFileSavePathBase(savePathService);