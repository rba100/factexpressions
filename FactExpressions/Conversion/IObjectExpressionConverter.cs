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
        INoun Get(object obj);
        INoun GetPossessive(object subject, PropertyInfo info);

        Pronoun GetPronoun(object obj);
        IExpression FromPropertyDifferences(object subject, IEnumerable<PropertyDifference> differences);
        void AddPronoun<T>(Func<T, Pronoun> describer);
    }
}