using AirTransfer.Interfaces;
using AirTransfer.Interfaces.Impls;


namespace AirTransfer;

public sealed class FileSavePath(ISavePathService savePathService) : DeskTopFileSavePathBase(savePathService);