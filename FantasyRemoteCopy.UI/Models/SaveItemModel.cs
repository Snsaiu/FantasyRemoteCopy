using CommunityToolkit.Mvvm.ComponentModel;

namespace FantasyRemoteCopy.UI.Models;

public partial class SaveItemModel : ObservableObject
{

    [ObservableProperty]
    private string guid;

    [ObservableProperty]
    private ImageSource image;


    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private string content;

    [ObservableProperty]
    private string sourceDeviceName;


    [ObservableProperty]
    private string time;

    [ObservableProperty]
    private bool isFile;

    [ObservableProperty]
    private bool isText;

}