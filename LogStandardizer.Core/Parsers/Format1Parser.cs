using LogStandardizer.Core.Models;
using System.Globalization;

namespace LogStandardizer.Core.Parsers;

public class Format1Parser : ILogParser
{
    public LogEntry? TryParse(string[] lines)
    {
        if (lines.Length < 4)
            return null;

        var firstLineParts = lines[0].Split(' ', 4, StringSplitOptions.RemoveEmptyEntries);
        if (firstLineParts.Length < 4)
            return null;

        if (!DateTime.TryParseExact($"{firstLineParts[0]} {firstLineParts[1]}", "dd.MM.yyyy HH:mm:ss.fff",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out var datetime))
            return null;

        var levelRaw = firstLineParts[2];
        var message = firstLineParts[3];

        string? dateLine = lines.FirstOrDefault(l => l.StartsWith("Дата:"));
        string? timeLine = lines.FirstOrDefault(l => l.StartsWith("Время:"));
        string? levelLine = lines.FirstOrDefault(l => l.StartsWith("УровеньЛогирования:"));
        string? messageLine = lines.FirstOrDefault(l => l.StartsWith("Сообщение:"));

        if (dateLine == null || timeLine == null || levelLine == null)
            return null;

        var dateStr = dateLine["Дата:".Length..].Trim();
        var timeStr = timeLine["Время:".Length..].Trim();
        var levelStr = levelLine["УровеньЛогирования:".Length..].Trim();
        var finalMessage = messageLine != null ? messageLine["Сообщение:".Length..].Trim() : message;

        if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            return null;

        var dateOnly = DateOnly.FromDateTime(dateTime);

        return new LogEntry
        {
            Date = dateOnly,
            Time = timeStr,
            LogLevel = NormalizeLevel(levelStr),
            CallerMethod = "DEFAULT",
            Message = finalMessage
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
