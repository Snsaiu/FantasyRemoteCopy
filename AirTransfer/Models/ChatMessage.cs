using AirTransfer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AirTransfer.Models;

public class MessagesItem
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonConverter(typeof(RoleConverter))]
    [JsonProperty("role")]
    public Role Role { get; set; }
}

public class ResponseFormat
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("type")]
    public string Type { get; set; }
}

public class StreamOption
{
    [JsonProperty("include_usage")] public bool IncludeUsage { get; set; }
}

public class MessageSession
{
    /// <summary>
    ///
    /// </summary>
    [JsonProperty("messages")]
    public List<MessagesItem> Messages { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("frequency_penalty")]
    public int FrequencyPenalty { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("max_tokens")]
    public int MaxTokens { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("presence_penalty")]
    public int PresencePenalty { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("response_format")]
    public ResponseFormat ResponseFormat { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("stop")]
    public List<string>? Stop { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("stream")]
    public bool Stream { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("stream_options")]
    public StreamOption StreamOptions { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("temperature")]
    public int Temperature { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("top_p")]
    public int TopP { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("tools")]
    public object Tools { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("tool_choice")]
    public string ToolChoice { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("logprobs")]
    public bool Logprobs { get; set; }

    /// <summary>
    ///
    /// </summary>
    [JsonProperty("top_logprobs")]
    public int TopLogprobs { get; set; }
}

public class RoleConverter : StringEnumConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        return Enum.Parse(typeof(Role), reader.Value.ToString());
    }
}