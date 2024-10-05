using CommunityToolkit.Mvvm.ComponentModel;
using FantasyRemoteCopy.UI.Enums;

namespace FantasyRemoteCopy.UI.Models;

public partial class SaveItemModel : ObservableObject
{

    [ObservableProperty]
    private string guid=String.Empty;
    
    [ObservableProperty]
    private string? title;

    [ObservableProperty]
    private string? content;

    [ObservableProperty]
    private string? sourceDeviceName;


    [ObservableProperty]
    private string? time;
    
    [ObservableProperty]
    private SendType sendType;

}