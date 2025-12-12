using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using FeedbackAnalyzer.API.Hubs;
using FeedbackAnalyzer.API.Services;
using FeedbackAnalyzer.Shared.Models;

namespace FeedbackAnalyzer.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;
    private readonly IHubContext<FeedbackHub> _hubContext;
    private readonly ILogger<FeedbackController> _logger;

    public FeedbackController(
        IFeedbackService feedbackService,
        IHubContext<FeedbackHub> hubContext,
        ILogger<FeedbackController> logger)
    {
        _feedbackService = feedbackService;
        _hubContext = hubContext;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<Feedback>> CreateFeedback([FromBody] CreateFeedbackRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest("Feedback text is required");
        }

        try
        {
            var feedback = await _feedbackService.CreateFeedbackAsync(request);

            // Broadcast the new feedback to all connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveFeedback", feedback);

            // Get updated sentiment trends and broadcast
            var trends = await _feedbackService.GetSentimentTrendsAsync();
            await _hubContext.Clients.All.SendAsync("ReceiveSentimentTrends", trends);

            return CreatedAtAction(nameof(GetFeedback), new { id = feedback.Id }, feedback);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating feedback");
            return StatusCode(500, "An error occurred while processing your feedback");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Feedback>> GetFeedback(int id)
    {
        var feedbacks = await _feedbackService.GetRecentFeedbackAsync(1000);
        var feedback = feedbacks.FirstOrDefault(f => f.Id == id);

        if (feedback == null)
        {
            return NotFound();
        }

        return feedback;
    }

    [HttpGet("recent")]
    public async Task<ActionResult<IEnumerable<Feedback>>> GetRecentFeedback([FromQuery] int count = 50)
    {
        var feedbacks = await _feedbackService.GetRecentFeedbackAsync(count);
        return Ok(feedbacks);
    }

    [HttpGet("trends")]
    public async Task<ActionResult<Dictionary<string, int>>> GetSentimentTrends()
    {
        var trends = await _feedbackService.GetSentimentTrendsAsync();
        return Ok(trends);
    }
}
