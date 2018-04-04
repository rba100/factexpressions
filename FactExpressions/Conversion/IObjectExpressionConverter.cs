﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace FactExpressions.Conversion
{
    public interface IObjectExpressionConverter
    {
        void AddDescriber<T>(Func<T, IExpression> describer);
        void AddDescriber<T>(Func<T, string> describer);
        IExpression Get(object obj);
        IExpression FromPropertyDifferences(Type objectType, IEnumerable<PropertyDifference> differences);
        IExpression GetPossessive(object subject, PropertyInfo info);

        Pronoun GetPronoun(object obj);
        IExpression FromPropertyDifferences(object subject, IEnumerable<PropertyDifference> differences);
        void AddPronoun<T>(Func<T, Pronoun> describer);
    }
}