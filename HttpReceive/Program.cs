// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

string url = "http://localhost:5001/text/";
HttpListener listener = new HttpListener();
listener.Prefixes.Add(url);


listener.Start();
Console.WriteLine($"HTTPS Server is listening at {url}");

while (true)
{
    var context = await listener.GetContextAsync();
    HttpListenerRequest request = context.Request;
    HttpListenerResponse response = context.Response;

    if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/text")
    {
        using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8))
        {
            string requestData = await reader.ReadToEndAsync();
            Console.WriteLine($"Received text data: {requestData}");
        }

        string responseString = "Text received successfully!";
        byte[] buffer = Encoding.UTF8.GetBytes(responseString);
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
    }
    else
    {
        response.StatusCode = (int)(request.HttpMethod != "POST" ? HttpStatusCode.MethodNotAllowed : HttpStatusCode.NotFound);
        response.Close();
    }
}