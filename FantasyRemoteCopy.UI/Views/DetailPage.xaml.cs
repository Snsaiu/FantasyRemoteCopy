using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Alerts;


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
        await Toast.Make("Success copy!", CommunityToolkit.Maui.Core.ToastDuration.Short, 12).Show();
        await Application.Current.MainPage.Navigation.PopAsync();
    }
}