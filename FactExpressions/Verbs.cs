using System;

namespace FactExpressions
{
    public enum Tense { Present, Past }

    public interface IVerb
    {
        string Conjugate(INounExpression subject, Tense tense);
    }

    public static class VerbExtensionMethods
    {
        public static string Conjugate(this IVerb verb, IExpression subject, Tense tense)
        {
            var nounExpression = new NounExpression(subject.ToString());
            return verb.Conjugate(nounExpression, tense);
        }
    }

    public interface IVerbExpression : IExpression
    {
        
    }

    public class VerbExpression : IVerbExpression
    {
        private readonly IVerb Verb;
        private readonly Tense Tense;
        private readonly IExpression Subject;
        private readonly IExpression Object;

        public VerbExpression(IVerb verb, Tense tense, IExpression subject, IExpression objct)
        {
            Verb = verb;
            Tense = tense;
            Subject = subject;
            Object = objct;
        }

        public VerbExpression(IVerb verb, Tense tense, IExpression subject)
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

    public static class Verbs
    {
        public static IVerb ToBe = new VerbToBe();
        public static IVerb ToGo = new VerbToGo();
        public static IVerb ToBecome = new VerbToBecome();
        public static IVerb ToHave = new VerbToHave();
        public static IVerb ToCreate = new VerbToCreate();
        public static IVerb ToRemove = new VerbToRemove();
        public static IVerb ToAlter = new VerbToAlter();
        public static IVerb ToOccur = new VerbToOccurr();
    }

    public class VerbToBe : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            switch (subject.Class)
            {
                case NounClass.I:
                    return tense == Tense.Present ? "am" : "was";
                case NounClass.You:
                    return tense == Tense.Present ? "are" : "were";
                case NounClass.It:
                    return tense == Tense.Present ? "is" : "was";
                default:
                    throw new ArgumentOutOfRangeException(nameof(subject.Class));
            }
        }
    }

    public class VerbToGo : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "went";

            return subject.Class == NounClass.It
                ? "goes"
                : "go";
        }
    }

    public class VerbToHave : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            switch (subject.Class)
            {
                case NounClass.I:
                    return tense == Tense.Present ? "have" : "had";
                case NounClass.You:
                    return tense == Tense.Present ? "have" : "had";
                case NounClass.It:
                    return tense == Tense.Present ? "has" : "had";
                default:
                    throw new ArgumentOutOfRangeException(nameof(subject.Class));
            }
        }
    }

    public class VerbToBecome : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "became";

            return subject.Class == NounClass.It
                ? "becomes"
                : "become";
        }
    }

    public class VerbToOccurr : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "occurred";

            return subject.Class == NounClass.It
                ? "occurs"
                : "occur";
        }
    }

    public class VerbToCreate : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "created";

            return subject.Class == NounClass.It
                ? "creates"
                : "create";
        }
    }

    public class VerbToRemove : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "removed";

            return subject.Class == NounClass.It
                ? "removes"
                : "remove";
        }
    }

    public class VerbToAlter : IVerb
    {
        public string Conjugate(INounExpression subject, Tense tense)
        {
            if (tense == Tense.Past) return "altered";

            return subject.Class == NounClass.It
                ? "alters"
                : "alter";
        }
    }
}