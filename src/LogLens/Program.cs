using System;
using System.IO;
using System.Linq;

namespace LogLens;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== LogLens v1.0 ===");

        // Just Use a default sample file if none provided via command line
        string filePath = args.Length > 0 ? args[0] : "sample.log";

        // Create a quick sample log file if it doesn't exist yet for easy testing
        if (!File.Exists(filePath))
        {
            File.WriteAllLines(filePath, new[]
            {
                "2026-10-28 14:20:05 | ERROR | Database connection failed",
                "2026-10-28 14:20:10 | INFO | Server started successfully",
                "2026-10-28 14:21:03 | WARNING | High memory usage detected",
                "This is a malformed log line and should be ignored safely.",
                "2026-10-28 14:22:01 | ERROR | Database connection failed",
                "2026-10-28 14:23:15 | ERROR | Timeout while reaching payment gateway"
            });
            Console.WriteLine($"[Info] Created a sample log file at '{filePath}'.\n");
        }

        var entries = LogParser.ParseFile(filePath);

        if (entries.Count == 0)
        {
            Console.WriteLine("No valid log entries found to analyze.");
            return;
        }

        // Summary counts
        var errorCount = entries.Count(e => e.Level == LogLevel.Error);
        var warningCount = entries.Count(e => e.Level == LogLevel.Warning);
        var infoCount = entries.Count(e => e.Level == LogLevel.Info);

        Console.WriteLine("--- Log Summary ---");
        Console.WriteLine($"Total Valid Entries: {entries.Count}");
        Console.WriteLine($"Errors:              {errorCount}");
        Console.WriteLine($"Warnings:            {warningCount}");
        Console.WriteLine($"Infos:               {infoCount}");

        // Most common errors(at least, i think)
        var topErrors = entries
            .Where(e => e.Level == LogLevel.Error)
            .GroupBy(e => e.Message)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .ToList();

        if (topErrors.Any())
        {
            Console.WriteLine("\n--- Most Common Errors ---");
            foreach (var group in topErrors)
            {
                Console.WriteLine($"- ({group.Count()}x) {group.Key}");
            }
        }
    }
}