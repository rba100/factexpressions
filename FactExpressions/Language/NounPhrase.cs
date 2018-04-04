using System;
using System.Collections.Generic;

namespace FactExpressions.Language
{
    public class NounPhrase : INoun
    {
        private readonly IReadOnlyCollection<IExpression> m_Expressions;

        public NounPhrase(IReadOnlyCollection<IExpression> expressions)
        {
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));

            if (expressions.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(expressions));

            m_Expressions = expressions;
        }

        public NounClass Class => NounClass.It;

        public bool IsPlural => m_Expressions.Count > 1;
    }
}