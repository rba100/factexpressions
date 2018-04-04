using System.Linq;

namespace FactExpressions.Language
{
    public class IndefiniteNoun : INoun
    {
        public readonly Noun Noun;

        public NounClass Class => Noun.Class;

        public bool IsPlural { get; set; }

        private static readonly char[] s_Vowels = {'a', 'e', 'i', 'o', 'u'};

        public IndefiniteNoun(Noun noun)
        {
            Noun = noun;
        }

        public IndefiniteNoun(string noun, NounClass nounClass = NounClass.It)
        {
            Noun =  new Noun(noun, nounClass);
        }

        public override string ToString()
        {
            var determiner = s_Vowels.Contains(Noun.Value.First())
                ? "an"
                : "a";

            return $"{determiner} {Noun}";
        }
    }
}