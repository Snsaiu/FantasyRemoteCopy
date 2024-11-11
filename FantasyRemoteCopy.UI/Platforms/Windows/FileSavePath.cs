using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;
using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;
namespace FantasyRemoteCopy.UI;


public sealed class FileSavePath(ISavePathService savePathService) : DeskTopFileSavePathBase(savePathService);