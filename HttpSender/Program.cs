// 配置 HttpClientHandler，忽略证书验证错误

using System.Text;

HttpClientHandler handler = new HttpClientHandler
{
    // 跳过 SSL 证书验证
    ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
};

using (HttpClient client = new HttpClient())
{
    // 设置目标服务器的 URL
    string serverUrl = "http://192.168.1.118:5001/text";

    // 要发送的文本内容
    string data = "Hello, this is a test message!";

    // 创建 HttpContent 实例，设置内容类型为 text/plain
    var content = new StringContent(data, Encoding.UTF8, "text/plain");

    try
    {
        // 发送 POST 请求
        HttpResponseMessage response = await client.PostAsync(serverUrl, content);

        // 确保请求成功
        response.EnsureSuccessStatusCode();

        // 读取服务器响应内容
        string responseData = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response from server: {responseData}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}