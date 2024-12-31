using AirTransfer.Interfaces.IConfigs;

namespace AirTransfer.Interfaces.Impls.Configs;

public sealed class ShowCloseDialogService : ConfigServiceBase, IShowCloseDialogService
{
    public override string Key => "ShowCloseDialog";
}