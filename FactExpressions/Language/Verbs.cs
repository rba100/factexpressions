using System;

namespace FactExpressions.Language
{
    public enum Tense { Present, Past }

    public interface IVerb
    {
        string Conjugate(INoun noun, Tense tense = Tense.Past);
    }

    public static class VerbExtensionMethods
    {
        public static string Conjugate(this IVerb verb, IExpression subject, Tense tense)
        {
            var nounExpression = new Noun(subject.ToString());
            return verb.Conjugate(nounExpression, tense);
        }

        public static string ConjugatePassive(this IVerb verb, INoun objct, Tense tense = Tense.Past)
        {
            return $"{objct} {Verbs.ToBe.Conjugate(objct, tense)} {verb.Conjugate(objct, tense)}";
        }
    }

    public static class Verbs
    {
        public static IVerb ToBe = new VerbToBe();
        public static IVerb ToGo = new VerbToGo();
        public static IVerb ToBecome = new VerbToBecome();
        public static IVerb ToHave = new VerbToHave();
        public static IVerb ToCreate = new VanillaVerb("create");
        public static IVerb ToRemove = new VanillaVerb("remove");
        public static IVerb ToAlter = new VanillaVerb("alter");
        public static IVerb ToOccur = new VerbToOccurr();
        public static IVerb ToReceive = new VanillaVerb("receive");
    }

    public class VerbToBe : IVerb
    {
        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            switch (noun.Class)
            {
                case NounClass.I:
                    return tense == Tense.Present ? "am" : "was";
                case NounClass.You:
                    return tense == Tense.Present ? "are" : "were";
                case NounClass.It:
                    return tense == Tense.Present ? "is" : "was";
                default:
                    throw new ArgumentOutOfRangeException(nameof(noun.Class));
            }
        }
    }

    public class VerbToGo : IVerb
    {
        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            if (tense == Tense.Past) return "went";

            return noun.Class == NounClass.It
                ? "goes"
                : "go";
        }
    }

    public class VerbToHave : IVerb
    {
        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            switch (noun.Class)
            {
                case NounClass.I:
                    return tense == Tense.Present ? "have" : "had";
                case NounClass.You:
                    return tense == Tense.Present ? "have" : "had";
                case NounClass.It:
                    return tense == Tense.Present ? "has" : "had";
                default:
                    throw new ArgumentOutOfRangeException(nameof(noun.Class));
            }
        }
    }

    public class VerbToBecome : IVerb
    {
        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            if (tense == Tense.Past) return "became";

            return noun.Class == NounClass.It
                ? "becomes"
                : "become";
        }
    }

    public class VerbToOccurr : IVerb
    {
        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            if (tense == Tense.Past) return "occurred";

            return noun.Class == NounClass.It
                ? "occurs"
                : "occur";
        }
    }

    public class VanillaVerb : IVerb
    {
        private readonly string m_Stem;

        public VanillaVerb(string stem)
        {
            m_Stem = stem;
        }

        public string Conjugate(INoun noun, Tense tense = Tense.Past)
        {
            if (tense == Tense.Past)
            {
                return m_Stem.EndsWith("e") ? $"{m_Stem}d" : $"{m_Stem}ed";
            }

            return noun.Class == NounClass.It
                ? $"{m_Stem}s"
                : m_Stem;
        }
    }
}