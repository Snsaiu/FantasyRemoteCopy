// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Text;

// 调用此方法启动服务器
await HttpServer.StartServer();

internal class HttpServer
{
    public static async Task StartServer()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://10.10.26.157:5001/"); // 使用局域网IP地址作为监听地址
        listener.Start();
        Console.WriteLine("服务器已启动，等待连接...");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;

            // 读取客户端发送的数据
            string requestData;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestData = await reader.ReadToEndAsync();
            }

            Console.WriteLine("收到请求数据: " + requestData);

            // 发送响应数据
            var response = context.Response;
            var responseString = "Hello from the server!";
            var buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }
    }
}