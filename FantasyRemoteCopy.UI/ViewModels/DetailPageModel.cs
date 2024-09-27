using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;

namespace FantasyRemoteCopy.UI.ViewModels
{
	public partial class DetailPageModel(IDialogService dialogService) : FantasyPageModelBase, INavigationAware
    {
        [ObservableProperty]
        private string content=string.Empty;

        [RelayCommand]
        public async Task Copy()
        {
            await Clipboard.Default.SetTextAsync(this.Content);
            await dialogService.DisplayAlert("Information", "Success copy!", "Ok");
            await Application.Current!.MainPage!.Navigation.PopAsync();
        }

        public void OnNavigatedFrom(string source, INavigationParameter parameter)
        {
        }

        public void OnNavigatedTo(string source, INavigationParameter parameter)
        {
            this.Content=  parameter.Get<string>("data");
        }
    }
}