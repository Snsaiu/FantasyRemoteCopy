namespace FantasyRemoteCopy.Core.Enums;

/// <summary>
/// 文本传输类型
/// </summary>
public enum TransformType
{
  
    /// <summary>
    /// 客户端群发进行设备发现
    /// </summary>
    ValidateAccount,
    /// <summary>
    /// 客户端接收设备发现并返回结果
    /// </summary>
    ReceiveValidateAccountResult,
    

    DataMeta,

    /// <summary>
    /// 请求建立tcp连接
    /// </summary>
    RequestBuildConnect,

    /// <summary>
    /// 建立完成
    /// </summary>
    BuildConnected,

    SendingTxtData,

    SendingFileData

       
}