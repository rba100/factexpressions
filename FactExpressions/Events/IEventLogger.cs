using System.Collections.Generic;

namespace FactExpressions.Events
{
    public interface IEventLogger
    {
        EventBuilder LogThat(object subject);
        void LogAsEvent(object obj);
        IList<object> EventItems();
    }
}