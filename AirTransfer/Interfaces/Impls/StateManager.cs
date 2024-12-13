namespace AirTransfer.Interfaces.Impls;

public class StateManager : IStateManager
{
    private readonly Dictionary<string, object> stateDictionary = [];

    public void SetState<T>(string key, T value)
    {
        if (value is null)
            throw new NullReferenceException($"“{nameof(value)}”不能为空");
        stateDictionary[key] = value;
    }

    public T GetState<T>(string key)
    {
        return !stateDictionary.TryGetValue(key, out var value)
            ? throw new ArgumentNullException($"字典中没有Key为{key}的值")
            : (T)value;
    }

    public bool ExistKey(string key)
    {
        return stateDictionary.ContainsKey(key);
    }
}