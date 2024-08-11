using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FantasyMvvm;
using FantasyMvvm.FantasyDialogService;
using FantasyMvvm.FantasyModels;

namespace FantasyRemoteCopy.UI.ViewModels
{
	public partial class DetailPageModel:FantasyPageModelBase,INavigationAware
	{

        [ObservableProperty]
        private string content;

        private readonly IDialogService _dialogService = null;

		public DetailPageModel(IDialogService dialogService)
		{
            this._dialogService = dialogService;
		}

        [RelayCommand]
        public async void Copy()
        {
            await Clipboard.Default.SetTextAsync(this.Content);
            await this._dialogService.DisplayAlert("Information", "Success copy!", "Ok");
            //  await this.DisplaySnackbar("Success copy!");
            //Toast.Make("Success copy!").Show();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public void OnNavigatedFrom(string source, INavigationParameter parameter)
        {
          this.Content=  parameter.Get<string>("data");
        }

        public void OnNavigatedTo(string source, INavigationParameter parameter)
        {
           
        }
    }
}

