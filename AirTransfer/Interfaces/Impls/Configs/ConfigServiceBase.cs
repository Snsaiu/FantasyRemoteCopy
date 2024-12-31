using AirTransfer.Interfaces.IConfigs;

namespace AirTransfer.Interfaces.Impls.Configs;

public abstract class ConfigServiceBase : IConfigService
{
    public abstract string Key { get; }

    public T Get<T>()
    {
        return Preferences.Default.Get(Key, default(T));
    }

    public void Set<T>(T value)
    {
        Preferences.Default.Set(Key, value);
    }
}