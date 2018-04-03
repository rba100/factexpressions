using System.Collections.Generic;
using System.Linq;

namespace FactExpressions
{
    public class CausalInstanceBuilder
    {
        private readonly IList<IVerbExpression> m_Causes = new List<IVerbExpression>();
        private readonly IList<IExpression> m_Consequences = new List<IExpression>();

        public static CausalInstanceBuilder From(IVerbExpression rootCause)
        {
            var builder = new CausalInstanceBuilder();
            builder.m_Causes.Add(rootCause);
            return builder;
        }

        public CausalInstanceBuilder ResultingIn(IExpression expression)
        {
            m_Consequences.Add(expression);
            return this;
        }

        public override string ToString()
        {
            if (m_Consequences.Any())
            {
                var consequences = new ConjunctionExpression(GenerateResults(), "because", GenerateCauses());
                return $"{consequences}";
            }
            else
            {
                return GenerateCauses().ToString();
            }
        }

        private IExpression GenerateCauses()
        {
            IExpression current = null;

            for (var index = 0; index < m_Causes.Count; index++)
            {
                if (current == null) current = m_Causes[index];
                else current = And(current, m_Causes[index]);
            }

            return current;
        }

        private IExpression GenerateResults()
        {
            IExpression current = null;

            for (var index = 0; index < m_Consequences.Count; index++)
            {
                if (current == null) current = m_Consequences[index];
                else current = And(current, m_Consequences[index]);
            }

            return current;
        }

        private IExpression And(IExpression left, IExpression right)
        {
            return new ConjunctionExpression(left, "and", right);
        }
    }

    public static class TransitionBuilder
    {
        public static CausalInstanceBuilder ToEvent(this IVerbExpression verbExpression)
        {
            return CausalInstanceBuilder.From(verbExpression);
        }

        public static IVerbExpression Became(this INounExpression beforeNoun, INounExpression afterNoun)
        {
            return new VerbExpression(Verbs.ToBecome, Tense.Past, beforeNoun, afterNoun);
        }

        public static IVerbExpression Had(this INounExpression beforeNoun, INounExpression afterNoun)
        {
            return new VerbExpression(Verbs.ToHave, Tense.Past, beforeNoun, afterNoun);
        }
    }
}
