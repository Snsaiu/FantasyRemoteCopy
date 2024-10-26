using System.Net;
using System.Text;
using FantasyRemoteCopy.UI.Enums;
using FantasyRemoteCopy.UI.Interfaces.Impls.Configs;
using FantasyRemoteCopy.UI.Interfaces.Impls.TcpTransfer;
using FantasyRemoteCopy.UI.Models;

namespace FantasyRemoteCopy.UI.Interfaces.Impls.HttpsTransfer;

public class HttpsLoopListenContent(FileSavePathBase fileSavePathBase)
    : LoopListenerBase<TransformResultModel<string>, ProgressValueModel, string>
{
    protected readonly FileSavePathBase FileSavePathBase = fileSavePathBase;

    public string WatchIp { get; set; }=String.Empty;
    
    public int Port { get; set; }
    
    public SendType ReceiveType { get; set; }
    
    public override async Task ReceiveAsync(Action<TransformResultModel<string>> receivedCallBack, IProgress<ProgressValueModel>? progress, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(WatchIp))
            throw new NullReferenceException("监听的IP不能为空");

        var listenner = new HttpListener();

        if (ReceiveType == SendType.Text)
        {
            listenner.Prefixes.Add($"http://localhost:{Port}/text/");
            listenner.Start();
            while (true)
            {
                var context = await listenner.GetContextAsync();
                var request = context.Request;
                var response = context.Response;
                if (request.HttpMethod == "POST")
                {
                    // 读取请求内容（假设文本为 UTF-8 编码）
                    using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
                    {
                        string requestData = await reader.ReadToEndAsync();
                        Console.WriteLine($"Received text data: {requestData}");
                        return;
                    }

                  
                }

            }
            
        }else if (ReceiveType == SendType.File)
        {
            
        }
        else
        {
            // 文件夹
        }
    }
}