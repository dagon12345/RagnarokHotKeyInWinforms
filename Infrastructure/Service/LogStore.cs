using Infrastructure.Model;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Infrastructure.Service
{
    public static class LogStore
    {
        private static BindingList<LogEntry> _entries = new BindingList<LogEntry>();

        public static BindingList<LogEntry> Entries => _entries;

        public static IEnumerable<LogEntry> SortedDescending =>
            _entries.OrderByDescending(e => e.TimeStamp);
    }

}
