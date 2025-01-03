namespace AirTransfer.Models;

//如果好用，请收藏地址，帮忙分享。
public class Message
{
    /// <summary>
    ///
    /// </summary>
    public string role { get; set; }

    /// <summary>
    /// 你好！很高兴见到你，有什么我可以帮忙的吗？
    /// </summary>
    public string content { get; set; }
}

public class ChoicesItem
{
    /// <summary>
    ///
    /// </summary>
    public int index { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Message message { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string logprobs { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string finish_reason { get; set; }
}

public class Usage
{
    /// <summary>
    ///
    /// </summary>
    public int prompt_tokens { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int completion_tokens { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int total_tokens { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int prompt_cache_hit_tokens { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int prompt_cache_miss_tokens { get; set; }
}

public class Root
{
    /// <summary>
    ///
    /// </summary>
    public string id { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string @object { get; set; }

    /// <summary>
    ///
    /// </summary>
    public int created { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string model { get; set; }

    /// <summary>
    ///
    /// </summary>
    public List<ChoicesItem> choices { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Usage usage { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string system_fingerprint { get; set; }
}