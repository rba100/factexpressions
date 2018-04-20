using System;
using System.Collections.Generic;
using System.Linq;

namespace FactExpressions.Events
{
    public class EventLogger : IScopingEventLogger
    {
        internal readonly List<Event> Events = new List<Event>();

        public EventBuilder LogThat(object subject)
        {
            return new EventBuilder(this, subject);
        }

        public EventBuilder And(object subject)
        {
            return new EventBuilder(this, subject);
        }

        public IScopingEventLogger LogEvent(object subject)
        {
            var evnt = new Event(subject);
            Events.Add(evnt);
            return this;
        }

        public IList<Event> EventItems()
        {
            return Events.Select(e => e.Copy()).ToList();
        }

        public IEventScope GetChildLogger()
        {
            return new EventScope(Events.Last()
                ?? throw new InvalidOperationException("Cannot open a child logger without an event to scope to."));
        }

        public EventBuilder AndThus(object subject)
        {
            return GetChildLogger().LogThat(subject);
        }
    }

    public interface IScopingEventLogger : IEventLogger
    {
        IEventScope GetChildLogger();

        EventBuilder AndThus(object subject);
    }

    public interface IEventScope : IScopingEventLogger
    {

    }

    internal class EventScope : IEventScope
    {
        private readonly IList<Event> Events;

        internal EventScope(Event evnt)
        {
            Events = evnt.Children;
        }

        public EventBuilder LogThat(object subject)
        {
            return new EventBuilder(this, subject);
        }

        public EventBuilder And(object subject)
        {
            return new EventBuilder(this, subject);
        }

        public IScopingEventLogger LogEvent(object subject)
        {
            Events.Add(new Event(subject));
            return this;
        }

        public IList<Event> EventItems()
        {
            throw new InvalidOperationException("EventItems can only be queried from the root scope.");
        }

        public IEventScope GetChildLogger()
        {
            return new EventScope(Events.Last());
        }

        public EventBuilder AndThus(object subject)
        {
            return GetChildLogger().LogThat(subject);
        }
    }
}