using Microsoft.FluentUI.AspNetCore.Components;

namespace AirTransfer.Interfaces;

public interface IThemeService
{
    DesignThemeModes GetDesignTheme();

    OfficeColor GetThemeColor();

    void SetThemeColor(OfficeColor color);

    void SetDesignTheme(DesignThemeModes theme);
}