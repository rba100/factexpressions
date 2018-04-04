using System;
using System.Collections.Generic;
using System.Linq;

namespace FactExpressions
{
    public enum NounClass { I, You, It }

    public interface INounExpression : IExpression
    {
        NounClass Class { get; }
        bool IsPlural { get; }
    }

    public class NounExpression : INounExpression
    {
        public NounClass Class { get; }

        public bool IsPlural { get; set; }

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

        public bool IsPlural { get; set; }

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

        public bool IsPlural { get; set; }

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

    public class NounPhrase : INounExpression
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

    public class PossessiveExpression : INounExpression
    {
        public NounClass Class => Possession.Class;

        public bool IsPlural => Possession.IsPlural;

        public INounExpression Owner { get; }

        public INounExpression Possession { get; }

        public PossessiveExpression(IExpression owner, INounExpression possession)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            Owner = owner as INounExpression ?? throw new ArgumentException($"{nameof(owner)} must be a noun expression", nameof(owner));
            Possession = possession ?? throw new ArgumentNullException(nameof(possession));
        }

        public override string ToString()
        {
            var suffix = Owner.ToString().EndsWith("s") ? "'" : "'s";

            return $"{Owner}{suffix} {Possession}";
        }
    }
}
