// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Security.Cryptography;
using System.Text;

// 调用此方法启动服务器
await HttpServer.StartServer();

internal class HttpServer
{
    private static string GenerateHMACSignatureForMessage(string message, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            return Convert.ToBase64String(hash);
        }
    }

    private static string GenerateHMACSignatureForFile(string filePath, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var fileBytes = File.ReadAllBytes(filePath);
            var hash = hmac.ComputeHash(fileBytes);
            return Convert.ToBase64String(hash);
        }
    }

    private static bool VerifyHMACSignature(string message, string signature, string key)
    {
        using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
            var computedSignature = Convert.ToBase64String(computedHash);
            return computedSignature == signature; // 验证签名
        }
    }


    public static async Task StartServer()
    {
        var listener = new HttpListener();
        listener.Prefixes.Add("http://10.10.26.157:5001/text/"); // 使用局域网IP地址作为监听地址
        listener.Start();
        Console.WriteLine("服务器已启动，等待连接...");

        while (true)
        {
            var context = await listener.GetContextAsync();
            var request = context.Request;

            var receivedSignature = request.Headers["X-Signature"];

            var secretKey = "saiu"; // 共享密钥


            // 读取客户端发送的数据
            string requestData;
            using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                requestData = await reader.ReadToEndAsync();
            }

            if (VerifyHMACSignature(requestData, receivedSignature, secretKey))
            {
                // 处理有效请求
                Console.WriteLine("Valid request received.");
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                using (var writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write("Request is valid");
                }
            }
            else
            {
                // 处理无效请求
                Console.WriteLine("Invalid request.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
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