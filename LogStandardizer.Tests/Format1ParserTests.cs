using LogStandardizer.Core.Models;
using LogStandardizer.Core.Parsers;
using Xunit;

namespace LogStandardizer.Tests;

public class Format1ParserTests
{
    private readonly Format1Parser _parser = new();

    [Fact]
    public void Parse_ValidFormat1_ReturnsLogEntry()
    {
        var lines = new[]
        {
            "10.03.2025 15:14:49.523 INFORMATION Версия программы: '3.4.0.48729'",
            "Дата: 10.03.2025",
            "Время: 15:14:49.523",
            "УровеньЛогирования: INFORMATION",
            "Сообщение: Версия программы: '3.4.0.48729'"
        };

        var entry = _parser.TryParse(lines);

        Assert.NotNull(entry);
        Assert.Equal("INFO", entry!.LogLevel);
        Assert.Equal("DEFAULT", entry.CallerMethod);
        Assert.Equal("Версия программы: '3.4.0.48729'", entry.Message);
        Assert.Equal("15:14:49.523", entry.Time);
        Assert.Equal(new DateOnly(2025, 3, 10), entry.Date);
    }

    [Fact]
    public void Parse_InvalidFormat1_ReturnsNull()
    {
        var lines = new[]
        {
            "Просто текст без разделителей"
        };

        var entry = _parser.TryParse(lines);

        Assert.Null(entry);
    }
}
