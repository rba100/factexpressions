using System.Collections.Generic;

namespace FactExpressions.Events
{
    public enum EventDetailTypes { Created, Removed, Altered, Became,
        Received
    }

    public class Event
    {
        public IReadOnlyCollection<EventDetail> EventDetails { get; }
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
        private readonly EventLogger m_EventLogger;
        private readonly object m_Subject;

        public EventBuilder(EventLogger eventLogger, object subject)
        {
            m_EventLogger = eventLogger;
            m_Subject = subject;
        }

        public EventLogger Created(object obj)
        {
            m_EventLogger.Details.Add(new EventDetail(m_Subject, obj, EventDetailTypes.Created));
            return m_EventLogger;
        }

        public EventLogger Removed(object obj)
        {
            m_EventLogger.Details.Add(new EventDetail(m_Subject, obj, EventDetailTypes.Removed));
            return m_EventLogger;
        }

        public EventLogger Altered(object obj)
        {
            m_EventLogger.Details.Add(new EventDetail(m_Subject, obj, EventDetailTypes.Altered));
            return m_EventLogger;
        }

        public EventLogger Became(object obj)
        {
            m_EventLogger.Details.Add(new EventDetail(m_Subject, obj, EventDetailTypes.Became));
            return m_EventLogger;
        }

        public EventLogger WasCreated()
        {
            m_EventLogger.Details.Add(new EventDetail(null, m_Subject, EventDetailTypes.Created));
            return m_EventLogger;
        }

        public EventLogger WasReceived()
        {
            m_EventLogger.Details.Add(new EventDetail(null, m_Subject, EventDetailTypes.Received));
            return m_EventLogger;
        }
    }
}
