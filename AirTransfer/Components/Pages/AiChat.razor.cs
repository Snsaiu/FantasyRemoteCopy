using System.Diagnostics;
using AirTransfer.Enums;
using AirTransfer.Interfaces.IConfigs;
using AirTransfer.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace AirTransfer.Components.Pages;

public partial class AiChat : PageComponentBase
{
    [Inject] protected HttpClient HttpClient { get; set; } = null!;
    [Inject] protected IAiSettingService AiService { get; set; } = null!;

    private OpenAiApiModel aiModel;

    private List<ChatModel> chatmodels;

    private MessageSession messageSession = new();


    protected override Task OnPageInitializedAsync(string? url, Dictionary<string, object>? data)
    {
        // 如果没有配置，那需要直接警告用户去配置
        aiModel = AiService.Query();
        messageSession.Messages.Add(new() { Content = "你是一个出色的助手,请每次回答都要用中文。", Role = Role.System });
        messageSession.Temperature = aiModel.Temperature;
        messageSession.TopP = aiModel.TopP;
        messageSession.Model = aiModel.CustomModelName;
        return Task.CompletedTask;
    }

    private async Task SendCommand(string arg)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{aiModel.ApiDomain}/chat/completions");
        request.Headers.Add("Accept", "application/json");
        request.Headers.Add("Authorization", $"Bearer {aiModel.ApiKey}");
        var newMessage = new MessagesItem() { Content = arg, Role = Role.User };
        messageSession.Messages.Add(newMessage);
        var json = JsonConvert.SerializeObject(messageSession);

        var content = new StringContent(
            json,
            null, "application/json");
        request.Content = content;
        var response = await HttpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Debug.WriteLine(await response.Content.ReadAsStringAsync());
    }
}