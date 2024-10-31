using System.Net.Sockets;
using System.Text;
using FantasyRemoteCopy.UI.Consts;
using FantasyRemoteCopy.UI.Models;
using Newtonsoft.Json;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;

/// <summary>
///     tcp发送文本基类
/// </summary>
public abstract class TcpSendTextBase : TcpSendBase<SendTextModel, ProgressValueModel>
{
    protected override async Task SendProcessAsync(NetworkStream sender, SendTextModel message,
        IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message.Text);
        await sender.WriteAsync(messageBytes, 0, (int)message.Size, cancellationToken);
        progress?.Report(new ProgressValueModel(message.Flag, message.TargetFlag, 1));
    }

    /// <summary>
    /// 发送文本并等待返回结果
    /// </summary>
    /// <param name="message">发送的消息</param>
    /// <param name="cancellationToken">取消</param>
    /// <typeparam name="T">返回结果</typeparam>
    /// <returns>返回数据结果</returns>
    /// <exception cref="Exception">json解析失败抛出异常</exception>
    public async Task<SendTextModel> SendAsync(SendTextModel message,CancellationToken cancellationToken=default)
    {
        var client = new TcpClient();
        try
        {
            await client.ConnectAsync(message.TargetFlag, message.Port, cancellationToken);

            var stream = client.GetStream();
            await SendMetadataMessageAsync(stream, message, cancellationToken);
            await SendProcessAsync(stream, message, null, cancellationToken);

           var buffer = new byte[2048];
           

           var byteRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
          
           var receiveData = Encoding.UTF8.GetString(buffer, 0, byteRead);
           
           return JsonConvert.DeserializeObject<SendTextModel>(receiveData) ?? throw new Exception();
        }
        finally
        {
            client.Close();
        }
    }
}