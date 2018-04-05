namespace FactExpressions.Language
{
    public class VerbExpression : IVerbExpression
    {
        private readonly IVerb Verb;
        private readonly Tense Tense;
        private readonly INoun Subject;
        private readonly INoun Object;

        public VerbExpression(
            IVerb verb,
            INoun subject = null,
            INoun objct = null,
            Tense tense = Tense.Past)
        {
            Verb = verb;
            Tense = tense;
            Subject = subject;
            Object = objct;
        }

        public override string ToString()
        {
            if (Subject == null)
            {
                return Verb.ConjugatePassive(Object, Tense);
            }

            if (Object == null)
            {
                return $"{Subject} {Verb.Conjugate(Subject, Tense)}";
            }

            return $"{Subject} {Verb.Conjugate(Subject, Tense)} {Object}";
        }
    }
}