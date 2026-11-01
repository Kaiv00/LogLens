using System;
using System.IO;
using Xunit;
using LogLens;

namespace LogLens.Tests;

public class LogParserTests
{
    [Fact]
    public void ParseLine_ValidLine_ReturnsCorrectLogEntry()
    {
        // Arrange
        string line = "2026-10-28 14:20:05 | ERROR | Database connection failed";

        // Act
        var entry = LogParser.ParseLine(line);

        // Assert
        Assert.NotNull(entry);
        Assert.Equal(new DateTime(2026, 10, 28, 14, 20, 5), entry.Timestamp);
        Assert.Equal(LogLevel.Error, entry.Level);
        Assert.Equal("Database connection failed", entry.Message);
    }

    [Fact]
    public void ParseLine_MalformedLine_ReturnsNull()
    {
        // Arrange
        string line = "This is a completely invalid log line with no pipes.";

        // Act
        var entry = LogParser.ParseLine(line);

        // Assert
        Assert.Null(entry);
    }

    [Fact]
    public void ParseLine_InvalidTimestamp_ReturnsNull()
    {
        // Arrange
        string line = "Not-A-Date | WARNING | High CPU usage";

        // Act
        var entry = LogParser.ParseLine(line);

        // Assert
        Assert.Null(entry);
    }

    [Fact]
    public void ParseFile_NonExistentFile_ReturnsEmptyListWithoutCrashing()
    {
        // Arrange
        string nonExistentPath = "non_existent_file_12345.log";

        // Act
        var entries = LogParser.ParseFile(nonExistentPath);

        // Assert
        Assert.NotNull(entries);
        Assert.Empty(entries);
    }
}