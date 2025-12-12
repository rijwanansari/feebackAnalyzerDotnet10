using Azure;
using Azure.AI.TextAnalytics;
using FeedbackAnalyzer.Shared.Models;

namespace FeedbackAnalyzer.API.Services;

public class AzureSentimentAnalysisService : ISentimentAnalysisService
{
    private readonly TextAnalyticsClient _client;
    private readonly ILogger<AzureSentimentAnalysisService> _logger;

    public AzureSentimentAnalysisService(IConfiguration configuration, ILogger<AzureSentimentAnalysisService> logger)
    {
        _logger = logger;
        var endpoint = configuration["AzureCognitiveServices:Endpoint"] ?? throw new InvalidOperationException("Azure Cognitive Services endpoint not configured");
        var apiKey = configuration["AzureCognitiveServices:ApiKey"] ?? throw new InvalidOperationException("Azure Cognitive Services API key not configured");

        _client = new TextAnalyticsClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    }

    public async Task<SentimentAnalysisResult> AnalyzeSentimentAsync(string text)
    {
        try
        {
            var response = await _client.AnalyzeSentimentAsync(text);
            var documentSentiment = response.Value;

            return new SentimentAnalysisResult
            {
                Sentiment = documentSentiment.Sentiment.ToString(),
                PositiveScore = documentSentiment.ConfidenceScores.Positive,
                NeutralScore = documentSentiment.ConfidenceScores.Neutral,
                NegativeScore = documentSentiment.ConfidenceScores.Negative
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing sentiment for text: {Text}", text);
            throw;
        }
    }
}
