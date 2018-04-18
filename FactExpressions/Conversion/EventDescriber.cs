using System;
using System.Collections.Generic;
using System.Linq;

using FactExpressions.Events;
using FactExpressions.Language;

namespace FactExpressions.Conversion
{
    public class EventDescriber
    {
        private readonly IObjectDescriber m_ObjectDescriber;
        private readonly ObjectPropertyComparer m_ObjectPropertyComparer = new ObjectPropertyComparer();

        public EventDescriber(IObjectDescriber objectDescriber)
        {
            m_ObjectDescriber = objectDescriber;
        }

        public IEnumerable<ExpressionTree> Describe(IEnumerable<Event> events)
        {
            foreach (var eventItem in events)
            {
                if (eventItem.Object is EventDetail eventDetail)
                {
                    yield return new ExpressionTree(FromEventDetail(eventDetail),
                                                    Describe(eventItem.Children));
                }
                else
                {
                    yield return new ExpressionTree(m_ObjectDescriber.GetNoun(eventItem.Object), 
                                                    Describe(eventItem.Children));
                }
            }
        }
        
        private IExpression FromEventDetail(EventDetail detail)
        {
            var obj = detail.Object == null ? null : m_ObjectDescriber.GetNoun(detail.Object);
            var sub = detail.Subject == null ? null : m_ObjectDescriber.GetNoun(detail.Subject);

            switch (detail.EventDetailType)
            {
                case EventDetailTypes.Created:
                    return new VerbExpression(Verbs.ToCreate, sub, obj);
                case EventDetailTypes.Removed:
                    return new VerbExpression(Verbs.ToRemove, sub, obj);
                case EventDetailTypes.Altered:
                    return new VerbExpression(Verbs.ToAlter, sub, obj);
                case EventDetailTypes.Became:
                    if (detail.Object != null && detail.Object.GetType() == detail.Subject?.GetType())
                    {
                        var diffs = m_ObjectPropertyComparer.Compare(detail.Subject, detail.Object).ToArray();
                        return diffs.Any()
                            ? m_ObjectDescriber.GetTransitionExpression(detail.Subject, diffs)
                            : m_ObjectDescriber.GetNoun(detail.Object);
                    }
                    return new VerbExpression(Verbs.ToBecome, sub, obj);
                case EventDetailTypes.Received:
                    return new VerbExpression(Verbs.ToReceive, sub, obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}