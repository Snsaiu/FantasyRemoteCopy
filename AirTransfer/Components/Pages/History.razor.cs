using AirTransfer.Interfaces;
using Microsoft.AspNetCore.Components;

using System.Collections.ObjectModel;
using AirTransfer.Enums;
using AirTransfer.Models;
using Microsoft.FluentUI.AspNetCore.Components;

namespace AirTransfer.Components.Pages;

public partial class History : PageComponentBase
{

    [Inject] private ISaveDataService SaveDataService { get; set; } = null!;
    [Inject] private IOpenFolder OpenFolder { get; set; } = null!;

    [Inject] private IDialogService DialogService { get; set; } = null!;


    [Parameter] public ObservableCollection<SaveItemModel>? Models { get; set; } = [];

    protected override async Task OnPageInitializedAsync(string? url, Dictionary<string, object>? data)
    {
        var list = await SaveDataService.GetAllAsync();

        if (list.Ok)
        {
            list.Data.Reverse();
            Rig(list.Data);
        }
        else
        {
            await DialogService.ShowWarningAsync(list.ErrorMsg ?? String.Empty, "Warning");
        }
    }

    private void Rig(List<SaveDataModel> sources)
    {
        Models?.Clear();
        foreach (var item in sources)
        {
            var sm = new SaveItemModel
            {
                Title = item.DataType == SendType.Text
                ? item.Content.Replace(" ", "").Length > 20
                    ? item.Content.Replace(" ", "").Replace("\n", "")[..20] + "..."
                    : item.Content.Replace(" ", "").Replace("\n", "")
                : Path.GetFileName(item.Content),
                SendType = item.DataType,
                Content = item.Content,
                Guid = item.Guid,
                SourceDeviceName = item.SourceDeviceNickName,
                Time = item.Time.ToString("yyyy-MM-dd HH:mm:ss")
            };

            Models?.Add(sm);
        }
    }
}