using System.Collections.Generic;
using System.Linq;

namespace FactExpressions.Events
{
    public enum EventDetailTypes {
        Created,
        Removed,
        Altered,
        Became,
        Received
    }

    public class Event
    {
        public object Object { get; }
        public IList<Event> Children { get; }

        public Event(object eventObject)
        {
            Object = eventObject ?? "null event";
            Children = new List<Event>();
        }

        public Event Copy()
        {
            var evnt = new Event(Object);
            Children.Select(c => c.Copy())
                    .ToList()
                    .ForEach(evnt.Children.Add);
            return evnt;
        }
    }

    public class EventDetail
    {
        public object Subject { get; }

        public object Object { get; }

        public EventDetailTypes EventDetailType { get; }

        public EventDetail(object subject, object o, EventDetailTypes eventDetailType)
        {
            Subject = subject;
            Object = o;
            EventDetailType = eventDetailType;
        }

        public EventDetail(object subject, EventDetailTypes eventDetailType)
        {
            Subject = subject;
            EventDetailType = eventDetailType;
        }
    }

    public class EventBuilder
    {
        private readonly IEventLogger m_EventLogger;
        private readonly object m_Subject;

        public EventBuilder(IEventLogger eventLogger, object subject)
        {
            m_EventLogger = eventLogger;
            m_Subject = subject;
        }

        public IScopingEventLogger Created(object obj)
        {
            return m_EventLogger.LogEvent(new EventDetail(m_Subject, obj, EventDetailTypes.Created));
        }

        public IScopingEventLogger Removed(object obj)
        {
            return m_EventLogger.LogEvent(new EventDetail(m_Subject, obj, EventDetailTypes.Removed));
        }

        public IScopingEventLogger Altered(object obj)
        {
            return m_EventLogger.LogEvent(new EventDetail(m_Subject, obj, EventDetailTypes.Altered));
        }

        public IScopingEventLogger Became(object obj)
        {
            return m_EventLogger.LogEvent(new EventDetail(m_Subject, obj, EventDetailTypes.Became));
        }

        public IScopingEventLogger WasCreated()
        {
            return m_EventLogger.LogEvent(new EventDetail(null, m_Subject, EventDetailTypes.Created));
        }

        public IScopingEventLogger WasReceived()
        {
            return m_EventLogger.LogEvent(new EventDetail(null, m_Subject, EventDetailTypes.Received));
        }
    }
}
