using FeedbackAnalyzer.Shared.Models;

namespace FeedbackAnalyzer.API.Services;

public interface ISentimentAnalysisService
{
    Task<SentimentAnalysisResult> AnalyzeSentimentAsync(string text);
}
