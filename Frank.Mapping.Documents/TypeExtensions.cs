namespace Frank.Mapping.Documents;

public static class TypeExtensions
{
    public static string GetFullName(this Type type)
    {
        var @namespace = type.Namespace;
        var name = type.Name;
        return @namespace == null ? name : $"{@namespace}.{name}";
    }
}