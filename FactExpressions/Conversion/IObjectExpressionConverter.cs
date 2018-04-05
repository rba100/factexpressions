using System;
using System.Collections.Generic;
using System.Reflection;
using FactExpressions.Language;

namespace FactExpressions.Conversion
{
    public interface IObjectExpressionConverter
    {
        void AddDescriber<T>(Func<T, INoun> describer);
        void AddDescriber<T>(Func<T, string> describer);
        void AddPronoun<T>(Func<T, Pronoun> describer);

        INoun Get(object obj);
        INoun GetPossessiveNoun(object owner, PropertyInfo owned);
        Pronoun GetPronoun(object obj);

        IExpression GetTransitionExpression(object subject, IEnumerable<PropertyDifference> differences);
    }
}