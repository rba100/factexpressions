using System;

namespace FactExpressions.Conversion
{
    public interface IObjectExpressionConverter
    {
        void AddDescriber<T>(Func<T, IExpression> describer);
        void AddDescriber<T>(Func<T, string> describer);
        IExpression Get(object obj);
    }
}