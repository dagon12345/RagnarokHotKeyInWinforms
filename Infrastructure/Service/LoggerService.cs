using Infrastructure.Model;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Service
{
    public static class LoggerService
    {
        private static ILogger _logger;

        public static void Configure(ILogger logger)
        {
            _logger = logger;
        }

        public static void LogError(Exception ex, string message)
        {
            var fullMessage = $"{message} {ex.Message}";
            _logger?.LogError(ex, message ?? ex.Message);
            LogStore.Entries.Add(new LogEntry 
            {
                TimeStamp = DateTime.UtcNow,
                Level = "Error",
                Message = fullMessage
            });
        }
        public static void LogInfo(string message)
        {
            _logger?.LogInformation(message);
            LogStore.Entries.Add(new LogEntry
            {
                TimeStamp = DateTime.UtcNow,
                Level = "Info",
                Message = message
            });
        }
    }
}
