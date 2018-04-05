namespace FactExpressions.Language
{
    public class Pronoun : INoun
    {
        public NounClass Class => NounClass.It;

        public bool IsPlural => false;

        public string AsSubject { get; }
        public string AsObject { get; }
        public string AsPossessive { get; }

        public Pronoun(string asSubject,
            string asObject,
            string asPossessive)
        {
            AsSubject = asSubject;
            AsObject = asObject;
            AsPossessive = asPossessive;
        }

        public override string ToString()
        {
            return AsSubject;
        }
    }

    public static class Pronouns
    {
        public static Pronoun Male = new Pronoun("he", "him", "his");
        public static Pronoun Female = new Pronoun("she", "her", "her");
        public static Pronoun GenderInvariant = new Pronoun("they", "them", "their");
        public static Pronoun It = new Pronoun("it", "it", "its");
    }
}