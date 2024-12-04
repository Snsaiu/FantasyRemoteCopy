﻿
namespace AirTransfer.Interfaces
{
    /// <summary>
    /// 获得本机ip
    /// </summary>
    public interface IGetLocalIp
    {
        Task<string> GetLocalIpAsync();
    }

}