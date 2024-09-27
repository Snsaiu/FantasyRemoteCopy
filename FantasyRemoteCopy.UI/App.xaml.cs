using FantasyMvvm;

namespace FantasyRemoteCopy.UI;

public partial class App : FantasyBootStarter
{
    protected override Window CreateWindow(IActivationState? activationState)
    {
        if (activationState is null)
            throw new NullReferenceException();

        var window = base.CreateWindow(activationState);
        window.Width = 400;
        window.Height = 600;
        return window;
    }

    protected override string CreateShell()
    {
        return "LoginPage";
    }
}