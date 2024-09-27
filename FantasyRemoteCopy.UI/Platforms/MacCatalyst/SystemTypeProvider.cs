using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public sealed class SystemTypeProvider:ISystemType
{
    public SystemType System { get; } = SystemType.MacOS;
}