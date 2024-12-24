using AirTransfer.Enums;
using AirTransfer.Interfaces;

namespace AirTransfer;

public sealed class SystemTypeProvider : ISystemType
{
    public SystemType System { get; } = SystemType.Windows;
}