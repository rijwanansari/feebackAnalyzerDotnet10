namespace FeedbackAnalyzer.Shared.Models;

public class Feedback
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Sentiment { get; set; } = string.Empty;
    public double SentimentScore { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Category { get; set; }
}
