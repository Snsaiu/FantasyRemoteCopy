﻿namespace AirTransfer.Interfaces
{
    public interface ISavePathService
    {
        void SavePath(string path);

        string? GetPath();
    }
}
