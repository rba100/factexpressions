using System.Collections.Generic;

namespace FactExpressions.Events
{
    public interface IEventLogger
    {
        EventBuilder LogThat(object subject);
        IScopingEventLogger LogEvent(object subject);
        IList<Event> EventItems();
    }
}