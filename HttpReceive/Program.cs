// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;
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
        // 创建一个TCP监听器，绑定到指定IP和端口
        TcpListener listener = new TcpListener(IPAddress.Parse("192.168.1.109"), 5001);
        listener.Start();
        Console.WriteLine("服务器已启动，等待连接...");

        while (true)
        {
            // 接受客户端连接
            using (TcpClient client = listener.AcceptTcpClient())
            {
                Console.WriteLine("客户端已连接.");
                NetworkStream stream = client.GetStream();

                // 读取请求
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("请求: \n" + request);

                // 检查请求路径
                if (request.Contains("GET /text"))
                {
                    // 构造HTTP响应
                    string response = "HTTP/1.1 200 OK\r\n" +
                                      "Content-Type: text/plain\r\n" +
                                      "Connection: close\r\n\r\n" +
                                      "Hello, World!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);

                    // 发送响应
                    stream.Write(responseBytes, 0, responseBytes.Length);
                    Console.WriteLine("响应已发送.");
                }
                else
                {
                    // 处理未找到路径的情况
                    string response = "HTTP/1.1 404 Not Found\r\n" +
                                      "Connection: close\r\n\r\n" +
                                      "Not Found";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    stream.Write(responseBytes, 0, responseBytes.Length);
                }
            }
        }
    }
}