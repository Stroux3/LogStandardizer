using LogStandardizer.Core.Models;

namespace LogStandardizer.Core.Parsers;

public interface ILogParser
{
    /// <summary>
    /// Пытается распарсить запись лога. Если не удалось, возвращает null.
    /// </summary>
    LogEntry? TryParse(string[] lines);
}
