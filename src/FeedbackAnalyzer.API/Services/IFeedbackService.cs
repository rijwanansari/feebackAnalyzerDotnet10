using FeedbackAnalyzer.Shared.Models;

namespace FeedbackAnalyzer.API.Services;

public interface IFeedbackService
{
    Task<Feedback> CreateFeedbackAsync(CreateFeedbackRequest request);
    Task<IEnumerable<Feedback>> GetRecentFeedbackAsync(int count = 50);
    Task<Dictionary<string, int>> GetSentimentTrendsAsync();
}
