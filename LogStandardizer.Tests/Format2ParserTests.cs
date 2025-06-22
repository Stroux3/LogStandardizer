using LogStandardizer.Core.Models;
using LogStandardizer.Core.Parsers;
using Xunit;

namespace LogStandardizer.Tests;

public class Format2ParserTests
{
    private readonly Format2Parser _parser = new();

    [Fact]
    public void Parse_ValidFormat2_ReturnsLogEntry()
    {
        var lines = new[]
        {
            "2025-03-10 15:14:51.5882| INFO|11|MobileComputer.GetDeviceId| Код устройства: '@MINDEO-M40-D-410244015546'"
        };

        var entry = _parser.TryParse(lines);

        Assert.NotNull(entry);
        Assert.Equal("INFO", entry!.LogLevel);
        Assert.Equal("MobileComputer.GetDeviceId", entry.CallerMethod);
        Assert.Equal("Код устройства: '@MINDEO-M40-D-410244015546'", entry.Message);
        Assert.Equal("15:14:51.5882", entry.Time);
        Assert.Equal(new DateOnly(2025, 3, 10), entry.Date);
    }

    [Fact]
    public void Parse_InvalidFormat2_ReturnsNull()
    {
        var lines = new[]
        {
            "просто текст без разделителей"
        };

        var entry = _parser.TryParse(lines);

        Assert.Null(entry);
    }
}
