using CommunityToolkit.Mvvm.ComponentModel;

using FantasyMvvm;

using FantasyRemoteCopy.UI.Language;

namespace FantasyRemoteCopy.UI.ViewModels.Base
{
    public abstract partial class ViewModelBase : FantasyPageModelBase
    {

        [ObservableProperty]
        private LocalizationResourceManager localizationResource = LocalizationResourceManager.Instance;
    }
}
