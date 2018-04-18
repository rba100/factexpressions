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

        public IEventScope OpenScope()
        {
            return new EventScope(Events.Last());
        }

        public EventBuilder AndThus(object subject)
        {
            return OpenScope().LogThat(subject);
        }
    }

    public interface IScopingEventLogger : IEventLogger
    {
        IEventScope OpenScope();
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

        public IScopingEventLogger LogEvent(object subject)
        {
            Events.Add(new Event(subject));
            return this;
        }

        public IList<Event> EventItems()
        {
            throw new InvalidOperationException("EventItems can only be queried from the root scope.");
        }

        public IEventScope OpenScope()
        {
            return new EventScope(Events.Last());
        }

        public EventBuilder AndThus(object subject)
        {
            return OpenScope().LogThat(subject);
        }
    }
}