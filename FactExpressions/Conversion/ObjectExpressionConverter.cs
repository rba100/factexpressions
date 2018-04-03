using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace FactExpressions.Conversion
{
    /// <summary>
    /// Maps Types to INounExpressions
    /// </summary>
    public class ObjectExpressionConverter : IObjectExpressionConverter
    {
        private readonly Dictionary<Type, object> m_Describers = new Dictionary<Type, object>();

        public void AddDescriber<T>(Func<T, string> describer)
        {
            var type = typeof(T);
            m_Describers.Add(type, describer);
        }

        public void AddDescriber<T>(Func<T, IExpression> describer)
        {
            var type = typeof(T);
            m_Describers.Add(type, describer);
        }

        public IExpression Get(object obj)
        {
            var type = obj.GetType();
            var describer = m_Describers.ContainsKey(type) ? m_Describers[type] : null;
            var converter = describer as Delegate;
            if (converter == null) return new NounExpression(obj.ToString());
            var result = converter.DynamicInvoke(obj);
            if (result is IExpression)
            {
                return result as IExpression;
            }
            return new NounExpression(result as string);
        }

        public IExpression FromPropertyDifferences(Type objectType, IEnumerable<PropertyDifference> differences)
        {
            var diffs = differences.ToArray();
            if(!diffs.Any()) throw new ArgumentException("There were no differences", nameof(differences));

            IEnumerable<IExpression> differenceExpressions = diffs.Select(d =>
                new VerbExpression
                (Verbs.ToBecome,
                    Tense.Past,
                    new NounExpression(d.Property.Name),
                    Get(d.Current)));

            return differenceExpressions.Aggregate((agg, next) => new ConjunctionExpression(agg, "and", next));
        }
    }
}