using System.Collections.Generic;
using System.Linq;

namespace FactExpressions.Events
{
    public class EventLogger : IEventLogger
    {
        internal readonly List<object> Details = new List<object>();

        public EventBuilder LogThat(object subject)
        {
            return new EventBuilder(this, subject);
        }

        public void LogAsEvent(object obj)
        {
            Details.Add(obj);
        }

        public IList<object> EventItems()
        {
            // Return copy
            return Details.ToList();
        }
    }
}