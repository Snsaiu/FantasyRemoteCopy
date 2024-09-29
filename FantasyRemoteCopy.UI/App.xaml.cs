using System.Globalization;
using FantasyMvvm;
using FantasyRemoteCopy.UI.Interfaces;
using FantasyRemoteCopy.UI.Language;

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
        InitLanguage();
        return "LoginPage";
    }

    private void InitLanguage()
    {
        var languageService = FantasyContainer.GetRequiredService<ILanguageService>();
        var language = languageService.GetLanguage();
        if (string.IsNullOrEmpty(language))
            return;
        LocalizationResourceManager.Instance.SetCulture(new CultureInfo(language));
    }
}