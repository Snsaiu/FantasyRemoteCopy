namespace FantasyRemoteCopy.UI.Consts;

public static class ConstParams
{
    /// <summary>
    /// 设备发现端口号
    /// </summary>
    public static readonly int INVITE_PORT = 5976;

    public static readonly int JOIN_PORT = 5977;

    public static readonly int TCP_PORT = 5978;

    public static class StateManagerKeys
    {
        public static readonly string ListenKey = "listen";

        public static readonly string DevicesKey = "devices";

        public static readonly string InformationModelKey = "information";
        
        public static readonly string IsWorkBusyKey= "isWorking";
    }

    public static string SaveFilePath()
    {
        var path = string.Empty;


#if WINDOWS

        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "FantasyRemoteCopy");
#else
        path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FantasyRemoteCopy");
#endif
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }
}