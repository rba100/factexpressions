using System;
using System.Linq;

namespace FactExpressions
{
    public enum NounClass { I, You, It }

    public interface INounExpression : IExpression
    {
        NounClass Class { get; }
    }

    public class NounExpression : INounExpression
    {
        public NounClass Class { get; }
        public readonly string Noun;

        public NounExpression(string noun, NounClass nounClass = NounClass.It)
        {
            Noun = noun;
            Class = nounClass;
        }

        public override string ToString()
        {
            return Noun;
        }
    }

    public class DefiniteNounExpression : INounExpression
    {
        public readonly NounExpression NounExpression;
        public NounClass Class => NounExpression.Class;

        public DefiniteNounExpression(NounExpression nounExpression)
        {
            NounExpression = nounExpression;
        }

        public override string ToString()
        {
            return $"the {NounExpression}";
        }
    }

    public class IndefiniteNounExpression : INounExpression
    {
        public readonly NounExpression NounExpression;
        public NounClass Class => NounExpression.Class;

        private static readonly char[] s_Vowels = {'a', 'e', 'i', 'o', 'u'};

        public IndefiniteNounExpression(NounExpression nounExpression)
        {
            NounExpression = nounExpression;
        }

        public IndefiniteNounExpression(string noun, NounClass nounClass = NounClass.It)
        {
            NounExpression =  new NounExpression(noun, nounClass);
        }

        public override string ToString()
        {
            var determiner = s_Vowels.Contains(NounExpression.Noun.First())
                ? "an"
                : "a";

            return $"{determiner} {NounExpression}";
        }
    }
}
