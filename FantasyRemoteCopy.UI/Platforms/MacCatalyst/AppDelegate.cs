﻿using Foundation;
using Foundation;

namespace FantasyRemoteCopy.UI;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp()
    {
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_sqlite3());
        return MauiProgram.CreateMauiApp();
    }
}