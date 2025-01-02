using AirTransfer.Models;
using Microsoft.AspNetCore.Components;

namespace AirTransfer.Components.Components;

public partial class ChatMessageList : ComponentBase
{
    private IEnumerable<ChatModel> messages = [];

    protected override void OnInitialized()
    {
        base.OnInitialized();
        messages = new List<ChatModel>
        {
            new ChatModel
            {
                Prompt = "Hello, how can I help you today?",
                ResponseStrings = new List<string>
                {
                    "I need help with my flight."
                }
            },
            new ChatModel
            {
                Prompt = "Sure, what do you need help with?",
                ResponseStrings = new List<string>
                {
                    "I need to change my flight."
                }
            },
            new ChatModel
            {
                Prompt = "I can help with that. What is your confirmation number?",
                ResponseStrings = new List<string>
                {
                    "123456789"
                }
            },
            new ChatModel
            {
                Prompt = "Thank you. What is your new flight date?",
                ResponseStrings = new List<string>
                {
                    "12/25/2021"
                }
            },
            new ChatModel
            {
                Prompt = "Thank you. Your flight has been changed to 12/25/2021. Is there anything else I can help you with?",
                ResponseStrings = new List<string>
                {
                    "No, thank you."
                }
            }
        };      
    }
}