using AirTransfer.Interfaces.IConfigs;

namespace AirTransfer.Interfaces.Impls.Configs;

public sealed class CloseAppBehaviorService : ConfigServiceBase, ICloseAppBehaviorService
{
    public override string Key => "CloseAppBehavior";
}