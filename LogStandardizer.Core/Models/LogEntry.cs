namespace LogStandardizer.Core.Models;

public class LogEntry
{
    public DateOnly Date { get; set; }
    public string? Time { get; set; }
    public string? LogLevel { get; set; }
    public string CallerMethod { get; set; } = "DEFAULT";
    public string? Message { get; set; }
}
