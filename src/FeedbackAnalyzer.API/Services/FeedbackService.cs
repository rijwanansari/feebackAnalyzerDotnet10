using FeedbackAnalyzer.API.Data;
using FeedbackAnalyzer.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackAnalyzer.API.Services;

public class FeedbackService : IFeedbackService
{
    private readonly FeedbackDbContext _context;
    private readonly ISentimentAnalysisService _sentimentService;
    private readonly ILogger<FeedbackService> _logger;

    public FeedbackService(
        FeedbackDbContext context,
        ISentimentAnalysisService sentimentService,
        ILogger<FeedbackService> logger)
    {
        _context = context;
        _sentimentService = sentimentService;
        _logger = logger;
    }

    public async Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request)
    {
        try
        {
            // Analyze sentiment
            var sentimentResult = await _sentimentService.AnalyzeSentimentAsync(request.Text);

            // Determine overall score (using positive score as primary indicator)
            double sentimentScore = sentimentResult.Sentiment switch
            {
                "Positive" => sentimentResult.PositiveScore,
                "Negative" => sentimentResult.NegativeScore,
                "Neutral" => sentimentResult.NeutralScore,
                _ => 0
            };

            // Create feedback entity
            var feedback = new Feedback
            {
                Text = request.Text,
                Category = request.Category,
                Sentiment = sentimentResult.Sentiment,
                SentimentScore = sentimentScore,
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created feedback with ID {Id} and sentiment {Sentiment}", feedback.Id, feedback.Sentiment);

            return feedback;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating feedback");
            throw;
        }
    }

    public async Task<IEnumerable<Feedback>> GetRecentFeedbackAsync(int count = 50)
    {
        return await _context.Feedbacks
            .OrderByDescending(f => f.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<Dictionary<string, int>> GetSentimentTrendsAsync()
    {
        var trends = await _context.Feedbacks
            .GroupBy(f => f.Sentiment)
            .Select(g => new { Sentiment = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Sentiment, x => x.Count);

        return trends;
    }
}
