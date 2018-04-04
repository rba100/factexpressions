namespace FactExpressions.Language
{
    public enum NounClass { I, You, It }

    public interface INoun : IExpression
    {
        NounClass Class { get; }
        bool IsPlural { get; }
    }
}
