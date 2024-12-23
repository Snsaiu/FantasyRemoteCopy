﻿using System.ComponentModel;
using System.Globalization;
using AirTransfer.Resources.Languages;

namespace AirTransfer.Language;

public class LocalizationResourceManager : INotifyPropertyChanged
{
    private LocalizationResourceManager()
    {
        AppResources.Culture = new("en-US");
    }

    public static LocalizationResourceManager Instance { get; } = new();

    public string this[string resourceKey] =>
        AppResources.ResourceManager.GetString(resourceKey, AppResources.Culture) ?? string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void SetCulture(CultureInfo culture)
    {
        AppResources.Culture = culture;
        PropertyChanged?.Invoke(this, new(null));
    }
}