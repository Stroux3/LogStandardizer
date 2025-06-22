using LogStandardizer.Core.Models;
using System.Globalization;

namespace LogStandardizer.Core.Parsers;

public class Format2Parser : ILogParser
{
    public LogEntry? TryParse(string[] lines)
    {
        if (lines.Length == 0)
            return null;

        var parts = lines[0].Split('|');
        if (parts.Length < 5)
            return null;

        var dateTimeStr = parts[0].Trim();
        if (!DateTime.TryParseExact(dateTimeStr, "yyyy-MM-dd HH:mm:ss.ffff", CultureInfo.InvariantCulture,
            DateTimeStyles.None, out var datetime))
            return null;

        var levelRaw = parts[1].Trim();
        var caller = parts[3].Trim();
        var message = parts[4].Trim();

        return new LogEntry
        {
            Date = DateOnly.FromDateTime(datetime),
            Time = datetime.ToString("HH:mm:ss.ffff"),
            LogLevel = NormalizeLevel(levelRaw),
            CallerMethod = string.IsNullOrWhiteSpace(caller) ? "DEFAULT" : caller,
            Message = message
        };
    }

    private static string NormalizeLevel(string raw) =>
        raw.ToUpperInvariant() switch
        {
            "INFORMATION" => "INFO",
            "WARNING" => "WARN",
            _ => raw.ToUpperInvariant()
        };
}
