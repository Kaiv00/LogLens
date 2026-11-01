# LogLens 🔍

A lightweight, robust command-line utility built in C# and .NET for parsing, analyzing, and reporting on log files safely.

## Features

- **Structured Parsing:** Parses timestamp, log level (`INFO`, `WARNING`, `ERROR`), and message content.
- **Fault-Tolerant:** Gracefully handles malformed lines, unknown log levels, empty spaces, and missing files without crashing.
- **Summary Analytics:** Reports total entry counts categorized by severity.
- **Error Spotting:** Highlights top recurring error messages to assist quick debugging.

## Project Structure

```text
LogLens/
├── LogLens.sln
├── src/
│   └── LogLens/
│       ├── LogEntry.cs
│       ├── LogLevel.cs
│       ├── LogParser.cs
│       └── Program.cs
└── tests/
    └── LogLens.Tests/
        └── LogParserTests.cs