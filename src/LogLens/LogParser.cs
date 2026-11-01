using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogLens;

public class LogParser
{
    public static List<LogEntry> ParseFile(string filePath)
    {
        var entries = new List<LogEntry>();

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"[Warning] File not found: {filePath}");
            return entries;
        }

        foreach (var line in File.ReadLines(filePath))
        {
            var entry = ParseLine(line);
            if (entry != null)
            {
                entries.Add(entry);
            }
        }

        return entries;
    }

    public static LogEntry? ParseLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
            return null;

        var parts = line.Split('|');
        if (parts.Length < 3)
            return null; // Malformed line (ignores garbage gracefully)

        if (!DateTime.TryParse(parts[0].Trim(), out var timestamp))
            return null;

        var levelStr = parts[1].Trim().ToUpperInvariant();
        LogLevel level = levelStr switch
        {
            "INFO" => LogLevel.Info,
            "WARNING" => LogLevel.Warning,
            "ERROR" => LogLevel.Error,
            _ => LogLevel.Info // Default fallback for unknown tags
        };

        // If the level string is completely invalid, treat it as malformed
        if (levelStr != "INFO" && levelStr != "WARNING" && levelStr != "ERROR")
            return null;

        var message = string.Join("|", parts.Skip(2)).Trim();

        return new LogEntry
        {
            Timestamp = timestamp,
            Level = level,
            Message = message
        };
    }
}