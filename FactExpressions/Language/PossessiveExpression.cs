using System;
using FactExpressions.Conversion;

namespace FactExpressions.Language
{
    public class Possessive : INoun
    {
        public NounClass Class => NounClass.It;

        public bool IsPlural => Possession is INoun && ((INoun) Possession).IsPlural;

        public INoun Owner { get; }

        public IExpression Possession { get; }

        public Possessive(IExpression owner, IExpression possession)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));
            Owner = owner as INoun ?? throw new ArgumentException($"{nameof(owner)} must be a noun expression", nameof(owner));
            Possession = possession ?? throw new ArgumentNullException(nameof(possession));
        }

        public override string ToString()
        {
            var suffix   = Owner.ToString().EndsWith("s") ? "'" : "'s";

            var possesor = Owner is Pronoun 
                ? ((Pronoun) Owner).AsPossessive
                : $"{Owner}{suffix}";

            return $"{possesor} {Possession}";
        }
    }
}