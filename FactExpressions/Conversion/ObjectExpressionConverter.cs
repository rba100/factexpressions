using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FactExpressions.Language;

namespace FactExpressions.Conversion
{
    /// <summary>
    /// Maps Types to INounExpressions
    /// </summary>
    public class ObjectExpressionConverter : IObjectExpressionConverter
    {
        private readonly Dictionary<Type, object> m_Describers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> m_Pronouns = new Dictionary<Type, object>();

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

        public void AddPronoun<T>(Func<T, Pronoun> describer)
        {
            var type = typeof(T);
            m_Pronouns.Add(type, describer);
        }

        public IExpression Get(object obj)
        {
            if(obj == null) return new Noun("null");

            var type = obj.GetType();
            var describer = m_Describers.ContainsKey(type) ? m_Describers[type] : null;
            var converter = describer as Delegate;
            if (converter == null) return new Noun(obj.ToString());
            var result = converter.DynamicInvoke(obj);
            if (result is IExpression)
            {
                return result as IExpression;
            }
            return new Noun(result as string);
        }

        public IExpression FromPropertyDifferences(Type objectType, IEnumerable<PropertyDifference> differences)
        {
            var diffs = differences.ToArray();
            if (!diffs.Any()) throw new ArgumentException("There were no differences", nameof(differences));

            IEnumerable<IExpression> differenceExpressions = diffs.Select(d =>
                new VerbExpression
                (Verbs.ToBecome,
                    new Noun(d.Property.Name),
                    Get(d.Current)));

            return differenceExpressions.Aggregate((agg, next) => new ConjunctionExpression(agg, "and", next));
        }

        public IExpression FromPropertyDifferences(object subject, IEnumerable<PropertyDifference> differences)
        {
            var subExpression = Get(subject) as INoun;
            var diffs = differences.ToArray();
            if (!diffs.Any()) throw new ArgumentException("There were no differences", nameof(differences));
            var diffExps = new List<IExpression>();
            for (int i = 0; i < diffs.Length; i++)
            {
                var sub = i == 0 ? subExpression : GetPronoun(subject);
                var poss = new Possessive(sub, new Noun(diffs[i].Property.Name.ToLowerInvariant()));
                diffExps.Add(new VerbExpression(Verbs.ToBecome, poss, Get(diffs[i].Current)));
            }

            return diffExps.Aggregate((agg, next) => new ConjunctionExpression(agg, "and", next));
        }

        public IExpression GetPossessive(object subject, PropertyInfo info)
        {
            return new Possessive(Get(subject), new Noun(info.Name));
        }

        public Pronoun GetPronoun(object obj)
        {
            var type = obj.GetType();
            var pronounFunc = m_Pronouns.ContainsKey(type) ? m_Pronouns[type] : null;
            var converter = pronounFunc as Delegate;
            if (converter == null) return new Pronoun("it", "it", "its");
            var result = converter.DynamicInvoke(obj);
            if (result is Pronoun)
            {
                return result as Pronoun;
            }
            return new Pronoun("it", "it", "its");
        }
    }

    public class Pronoun : INoun
    {
        public NounClass Class => NounClass.It;

        public bool IsPlural => false;

        public string AsSubject { get; }
        public string AsObject { get; }
        public string AsPossessive { get; }

        public Pronoun(string asSubject,
                       string asObject,
                       string asPossessive)
        {
            AsSubject = asSubject;
            AsObject = asObject;
            AsPossessive = asPossessive;
        }

        public override string ToString()
        {
            return AsSubject;
        }
    }

    public static class Pronouns
    {
        public static Pronoun Male = new Pronoun("he", "him", "his");
        public static Pronoun Female = new Pronoun("she", "her", "her");
        public static Pronoun GenderInvariant = new Pronoun("they", "them", "their");
        public static Pronoun It = new Pronoun("it", "it", "its");
    }
}