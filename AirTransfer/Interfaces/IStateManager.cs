namespace AirTransfer.Interfaces;

public interface IStateManager
{
    public void SetState<T>(string key, T value);

    public T GetState<T>(string key);

    public bool ExistKey(string key);
}