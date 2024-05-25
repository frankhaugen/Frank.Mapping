namespace Frank.Mapping.Documents;

public class ThrowHelper
{
    public static void ThrowAggregatedExceptionIfAny(params Exception[] exceptions)
    {
        Throw(exceptions);
    }
    
    public static void ThrowIfNullOrWhiteSpace(string value, string parameterName)
    {
        Throw(value, parameterName);
    }

    private static void Throw(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            Throw(new ArgumentException($"'{parameterName}' cannot be null or whitespace."));
    }

    private static void Throw(Exception exception) => throw exception;

    private static void Throw(Exception[] exceptions)
    {
        if (exceptions.Length == 0) return;
        throw new AggregateException(exceptions);
    }
}