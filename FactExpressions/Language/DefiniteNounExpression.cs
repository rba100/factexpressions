namespace FactExpressions.Language
{
    public class DefiniteNoun : INoun
    {
        public readonly Noun Noun;

        public NounClass Class => Noun.Class;

        public bool IsPlural { get; set; }

        public DefiniteNoun(Noun noun)
        {
            Noun = noun;
        }

        public override string ToString()
        {
            return $"the {Noun}";
        }
    }
}