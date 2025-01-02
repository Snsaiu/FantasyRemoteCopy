namespace AirTransfer.Models;

public enum ChatMessageType
{
    User,
    Bot
}

public class ChatModel
{
    public Guid ConversationId { get; set; }

    public string Content { get; set; } = string.Empty;

    public ChatMessageType ChatMessageType { get; set; }

    public DateTime Time { get; set; }
}