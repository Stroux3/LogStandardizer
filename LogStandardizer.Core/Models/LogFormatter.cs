namespace LogStandardizer.Core.Models;

public static class LogFormatter
{
    public static string Format(LogEntry entry)
    {
        string dateStr = entry.Date.ToString("dd-MM-yyyy");

        return string.Join(Environment.NewLine,
        [
            dateStr,
            entry.Time,
            entry.LogLevel,
            entry.CallerMethod,
            entry.Message
        ]);
    }
}
