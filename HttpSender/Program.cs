// 配置 HttpClientHandler，忽略证书验证错误

using System.Security.Cryptography;
using System.Text;

var handler = new HttpClientHandler
{
    // 跳过 SSL 证书验证
    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
};

using (var client = new HttpClient())
{
    // 设置目标服务器的 URL
    var serverUrl = "http://192.168.1.118:5001/text";


    var key = "saiu";

    // 要发送的文本内容
    var data = "Hello, this is a test message!";

    var signature = GenerateHMACSignatureForMessage(data, key);

    // 创建 HttpContent 实例，设置内容类型为 text/plain
    var content = new StringContent(data, Encoding.UTF8, "text/plain");

    try
    {
        client.DefaultRequestHeaders.Add("X-Signature", signature);
        // 发送 POST 请求
        var response = await client.PostAsync(serverUrl, content);

        // 确保请求成功
        response.EnsureSuccessStatusCode();

        // 读取服务器响应内容
        var responseData = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response from server: {responseData}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}


static string GenerateHMACSignatureForMessage(string message, string key)
{
    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
    {
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));
        return Convert.ToBase64String(hash);
    }
}

static string GenerateHMACSignatureForFile(string filePath, string key)
{
    using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
    {
        var fileBytes = File.ReadAllBytes(filePath);
        var hash = hmac.ComputeHash(fileBytes);
        return Convert.ToBase64String(hash);
    }
}