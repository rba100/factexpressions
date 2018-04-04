namespace FactExpressions
{
    public class PrepositionExpression : IExpression
    {
        private readonly string m_Preposition;
        private readonly IExpression m_PreExpression;
        private readonly IExpression m_PostExpression;

        public PrepositionExpression(IExpression preExpression,
                                     string preposition,
                                     IExpression postExpression)
        {
            m_Preposition = preposition;
            m_PreExpression = preExpression;
            m_PostExpression = postExpression;
        }

        public PrepositionExpression(IExpression preExpression, string preposition)
        {
            m_PreExpression = preExpression;
            m_Preposition = preposition;
        }

        public override string ToString()
        {
            return m_PostExpression == null
                ? $"{m_PreExpression} {m_Preposition}"
                : $"{m_PreExpression} {m_Preposition} {m_PostExpression}";
        }
    }

    public class ConjunctionExpression : IExpression
    {
        private readonly string m_Conjunction;
        private readonly IExpression m_PreExpression;
        private readonly IExpression m_PostExpression;

        internal ConjunctionExpression(IExpression preExpression, string conjunction, IExpression postExpression)
        {
            m_Conjunction = conjunction;
            m_PreExpression = preExpression;
            m_PostExpression = postExpression;
        }

        public override string ToString()
        {
            return $"{m_PreExpression} {m_Conjunction} {m_PostExpression}";
        }
    }
}