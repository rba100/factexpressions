using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using FactExpressions.Events;
using FactExpressions.Relations;

namespace FactExpressions.Conversion
{
    public class EventAnalyser
    {
        private readonly IRelationStore m_RelationStore;
        private readonly IObjectExpressionConverter m_ObjectExpressionConverter;
        private readonly ObjectPropertyComparer m_ObjectPropertyComparer = new ObjectPropertyComparer();

        public EventAnalyser(IRelationStore relationStore,
                             IObjectExpressionConverter objectExpressionConverter)
        {
            m_RelationStore = relationStore;
            m_ObjectExpressionConverter = objectExpressionConverter;
        }

        public IEnumerable<IExpression> GetExpressions(IEventLogger eventLogger)
        {
            var chains = new Dictionary<Type, ExpressionChain>();

            var items = eventLogger.EventItems();

            for (var i = 0; i < items.Count; i++)
            {
                object item = items[i];
                var eventDetails = item as EventDetail;
                if (eventDetails == null)
                {
                    var type = item.GetType();
                    var relatedTypes = m_RelationStore.GetSimpleRelations(type);

                    var chainKey = chains.Keys.FirstOrDefault(t => t == type)
                                   ?? chains.Keys.FirstOrDefault(t => relatedTypes.Contains(t));

                    bool top = false;
                    if (chainKey == null)
                    {
                        top = true;
                        chains[type] = new ExpressionChain();
                    }
                    var chain = chains[chainKey ?? type];

                    var verbExpression = top
                        ? new VerbExpression(Verbs.ToOccur, m_ObjectExpressionConverter.Get(item))
                        : m_ObjectExpressionConverter.Get(item);
                    chain.Add(verbExpression);
                }
                else
                {
                    var subjectType = eventDetails.Subject.GetType();
                    var objectType = eventDetails.Object?.GetType();

                    var relatedTypes = m_RelationStore.GetSimpleRelations(subjectType)
                                       .Union(m_RelationStore.GetSimpleRelations(objectType));

                    var chainKey = chains.Keys.FirstOrDefault(t => t == subjectType || t == objectType)
                                   ?? chains.Keys.FirstOrDefault(t => relatedTypes.Contains(t));
                    ExpressionChain chain;

                    if (chainKey == null)
                    {
                        chain = new ExpressionChain();
                        if (!chains.ContainsKey(subjectType)) chains[subjectType] = chain;
                        if (objectType != null && !chains.ContainsKey(objectType)) chains[objectType] = chain;
                    }
                    else
                    {
                        chain = chains[chainKey];
                    }

                    chain.Add(FromEventDetail(eventDetails));
                }
            }

            return chains.Values.Select(c => Collapse(c.Expressions(), "and"));
        }

        private IExpression FromEventDetail(EventDetail detail)
        {
            var obj = detail.Object == null ? null : m_ObjectExpressionConverter.Get(detail.Object);
            var sub = detail.Subject == null ? null : m_ObjectExpressionConverter.Get(detail.Subject);

            var subjectArgument = obj == null
                ? new VerbExpression(Verbs.ToBe, sub)
                : sub;

            switch (detail.EventDetailType)
            {
                case EventDetailTypes.Created:
                    return new VerbExpression(Verbs.ToCreate, subjectArgument, obj);
                case EventDetailTypes.Removed:
                    return new VerbExpression(Verbs.ToRemove, subjectArgument, obj);
                case EventDetailTypes.Altered:
                    return new VerbExpression(Verbs.ToAlter, subjectArgument, obj);
                case EventDetailTypes.Became:
                    if (detail.Object != null && detail.Object.GetType() == detail.Subject?.GetType())
                    {
                        var diffs = m_ObjectPropertyComparer.Compare(detail.Subject, detail.Object).ToArray();
                        var exp = diffs.Select(diff =>
                            new VerbExpression(Verbs.ToBecome,
                                m_ObjectExpressionConverter.GetPossessive(detail.Subject, diff.Property),
                                m_ObjectExpressionConverter.Get(diff.Current)));
                        return Collapse(exp, "and");
                    }
                    return new VerbExpression(Verbs.ToBecome, subjectArgument, obj);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IExpression Collapse(IEnumerable<IExpression> expressions, string conjuntion)
        {
            return expressions.Aggregate((aggregate, next) => aggregate == null ? next : new ConjunctionExpression(aggregate, conjuntion, next));
        }
    }

    public class ExpressionChain
    {
        private readonly IList<IExpression> m_OrderedExpressions
            = new List<IExpression>();

        public void Add(IExpression expression)
        {
            m_OrderedExpressions.Add(expression);
        }

        public IEnumerable<IExpression> Expressions()
        {
            return m_OrderedExpressions.ToArray();
        }
    }
}