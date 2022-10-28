using CommunityToolkit.Maui.Alerts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FantasyRemoteCopy.UI.Views;

public partial class DetailPage : ContentPage
{
    public DetailPage(string content)
    {
        InitializeComponent();
        this.edit.Text= content;
    }

    private async void CopyDataEvent(object sender, EventArgs e)
    {
        await Clipboard.Default.SetTextAsync(this.edit.Text);
        await this.DisplayAlert("Information", "Success copy!", "Ok");
      //  await this.DisplaySnackbar("Success copy!");
         //Toast.Make("Success copy!").Show();
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}