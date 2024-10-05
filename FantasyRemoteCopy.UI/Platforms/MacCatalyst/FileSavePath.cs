using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI;

public sealed class FileSavePath(ISavePathService savePathService) : DeskTopFileSavePathBase(savePathService);