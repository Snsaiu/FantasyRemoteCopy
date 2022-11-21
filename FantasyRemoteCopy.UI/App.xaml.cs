using FantasyMvvm;
namespace FantasyRemoteCopy.UI;

public partial class App : FantasyBootStarter
{

#if WINDOWS

protected override Window CreateWindow(IActivationState activationState)
    {
	    var window= activationState.Context.Services.GetService<Window>();
	    window.Width = 400;
	    window.Height = 600;
	    return window;
	    //return base.CreateWindow(activationState);
    }
#endif
	
    protected override string CreateShell()
    {
	    return "LoginPage";
    }
}

