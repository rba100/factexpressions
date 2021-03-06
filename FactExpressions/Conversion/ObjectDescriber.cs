using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FactExpressions.Language;

namespace FactExpressions.Conversion
{
    /// <summary>
    /// Maps objects to language constructs
    /// </summary>
    public class ObjectDescriber : IObjectDescriber
    {
        private readonly Dictionary<Type, object> m_Describers = new Dictionary<Type, object>();
        private readonly Dictionary<Type, object> m_Pronouns = new Dictionary<Type, object>();

        public void AddDescriber<T>(Func<T, string> describer)
        {
            var type = typeof(T);
            m_Describers.Add(type, describer);
        }

        public void AddDescriber<T>(Func<T, INoun> describer)
        {
            var type = typeof(T);
            m_Describers.Add(type, describer);
        }

        public void AddPronoun<T>(Func<T, Pronoun> describer)
        {
            var type = typeof(T);
            m_Pronouns.Add(type, describer);
        }

        public INoun GetNoun(object obj)
        {
            if (obj == null) return new Noun("null");

            var type = obj.GetType();
            var describer = m_Describers.ContainsKey(type) ? m_Describers[type] : null;
            if (describer == null)
            {
                var firstSimilarType = m_Describers.Keys.FirstOrDefault(k => k.IsAssignableFrom(type));
                describer = firstSimilarType != null 
                    ? m_Describers.ContainsKey(firstSimilarType)
                        ? m_Describers[firstSimilarType] : null 
                    : null;
            }

            if (!(describer is Delegate converter)) return new Noun(obj.ToString());
            var result = converter.DynamicInvoke(obj);
            if (result is INoun noun)
            {
                return noun;
            }
            return new Noun(result as string);
        }

        public IExpression GetTransitionExpression(object subject, IEnumerable<PropertyDifference> differences)
        {
            var subExpression = GetNoun(subject);
            var diffs = differences.ToArray();
            if (!diffs.Any()) throw new ArgumentException("There were no differences", nameof(differences));
            var diffExps = new List<IExpression>();
            for (int i = 0; i < diffs.Length; i++)
            {
                var sub = i == 0 ? subExpression : GetPronoun(subject);
                var poss = new Possessive(sub, GetNoun(diffs[i].Property));
                diffExps.Add(new VerbExpression(Verbs.ToBecome, poss, GetNoun(diffs[i].Current)));
            }
            return diffExps.Aggregate((agg, next) => new ConjunctionExpression(agg, "and", next));
        }

        public INoun GetPossessiveNoun(object owner, PropertyInfo owned)
        {
            return new Possessive(GetNoun(owner), GetNoun(owned));
        }

        public Pronoun GetPronoun(object obj)
        {
            var type = obj.GetType();
            var pronounFunc = m_Pronouns.ContainsKey(type) ? m_Pronouns[type] : null;
            if (!(pronounFunc is Delegate converter))
            {
                return Pronouns.It;
            }
            if (converter.DynamicInvoke(obj) is Pronoun pronoun)
            {
                return pronoun;
            }
            return Pronouns.It;
        }

        private INoun GetNoun(PropertyInfo propertyInfo)
        {
            var name = propertyInfo.Name;
            if (!name.Skip(1).Any(Char.IsUpper)) return new Noun(name.ToLowerInvariant());
            if (!name.Any(Char.IsLower)) return new Noun(name.ToLowerInvariant());

            IEnumerable<char> BreakByCamelCase(IEnumerable<char> chars)
            {
                bool firstDone = false;
                foreach (var c in chars)
                {
                    if (firstDone && Char.IsUpper(c)) yield return ' ';
                    yield return Char.ToLower(c);
                    firstDone = true;
                }
            }
            return new Noun(new string(BreakByCamelCase(name.ToCharArray()).ToArray()));
        }
    }
}