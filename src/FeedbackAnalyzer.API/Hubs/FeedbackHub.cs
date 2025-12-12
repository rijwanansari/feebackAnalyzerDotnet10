using Microsoft.AspNetCore.SignalR;
using FeedbackAnalyzer.Shared.Models;

namespace FeedbackAnalyzer.API.Hubs;

public class FeedbackHub : Hub
{
    private readonly ILogger<FeedbackHub> _logger;

    public FeedbackHub(ILogger<FeedbackHub> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendFeedbackUpdate(Feedback feedback)
    {
        await Clients.All.SendAsync("ReceiveFeedback", feedback);
    }

    public async Task SendSentimentTrendUpdate(Dictionary<string, int> trends)
    {
        await Clients.All.SendAsync("ReceiveSentimentTrends", trends);
    }
}
