using FantasyRemoteCopy.UI.Interfaces.Impls;

namespace FantasyRemoteCopy.UI;

public class DefaultScanLocalNetIp:LocalIpScannerBase
{

   protected override string Pattern { get; }=@"(?<ip>([0-9]{1,3}\.?){4})\s*(?<mac>([a-f0-9]{2}-?){6})";
}

