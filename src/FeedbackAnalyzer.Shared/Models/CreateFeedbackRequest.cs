namespace FeedbackAnalyzer.Shared.Models;

public class CreateFeedbackRequest
{
    public string Text { get; set; } = string.Empty;
    public string? Category { get; set; }
}
