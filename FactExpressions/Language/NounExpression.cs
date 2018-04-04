namespace FactExpressions.Language
{
    public class Noun : INoun
    {
        public NounClass Class { get; }

        public bool IsPlural { get; set; }

        public readonly string Value;

        public Noun(string noun, NounClass nounClass = NounClass.It)
        {
            Value = noun;
            Class = nounClass;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}