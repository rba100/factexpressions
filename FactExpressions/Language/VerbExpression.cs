namespace FactExpressions.Language
{
    public class VerbExpression : IVerbExpression
    {
        private readonly IVerb Verb;
        private readonly Tense Tense;
        private readonly IExpression Subject;
        private readonly IExpression Object;

        public VerbExpression(IVerb verb, IExpression subject, IExpression objct, Tense tense = Tense.Past)
        {
            Verb = verb;
            Tense = tense;
            Subject = subject;
            Object = objct;
        }

        public VerbExpression(IVerb verb, IExpression subject, Tense tense = Tense.Past)
        {
            Verb = verb;
            Tense = tense;
            Subject = subject;
        }

        public override string ToString()
        {
            if (Object != null)
                return $"{Subject} {Verb.Conjugate(Subject, Tense)} {Object}";
            return $"{Subject} {Verb.Conjugate(Subject, Tense)}";
        }
    }
}