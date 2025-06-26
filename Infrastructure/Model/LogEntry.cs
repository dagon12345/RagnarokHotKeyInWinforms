using System;

namespace Infrastructure.Model
{
    public class LogEntry
    {
        public DateTime TimeStamp { get; set; }
        public string Level { get; set; } //Info, error, etc.
        public string Message { get; set; }

        public override string ToString()
        {
            return $"[{TimeStamp:HH:mm:ss}] {Level}: {Message}";
        }
    }
}
