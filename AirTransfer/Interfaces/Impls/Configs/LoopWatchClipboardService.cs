namespace AirTransfer.Interfaces.Impls.Configs;

public class LoopWatchClipboardService(IStateManager stateManager) : ILoopWatchClipboardService
{

    public void SetState(bool state)
    {
        stateManager.SetState(Consts.ConstParams.StateManagerKeys.LoopWatchClipboardKey, state);
        Preferences.Default.Set(Consts.ConstParams.StateManagerKeys.LoopWatchClipboardKey, state);

    }

    public bool GetState()
    {
        return Preferences.Default.Get(Consts.ConstParams.StateManagerKeys.LoopWatchClipboardKey, false);
    }
}