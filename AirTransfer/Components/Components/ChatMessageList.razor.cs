using AirTransfer.Models;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Components;

public partial class ChatMessageList : ComponentBase
{
    private IEnumerable<ChatModel> messages = [];
}