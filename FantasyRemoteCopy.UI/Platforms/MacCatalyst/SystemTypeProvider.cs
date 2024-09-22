using FantasyRemoteCopy.Core.Enums;
using FantasyRemoteCopy.UI.Interfaces;

namespace FantasyRemoteCopy.UI;

public class SystemTypeProvider:ISystemType
{
    public SystemType System { get; } = SystemType.MacOS;
}