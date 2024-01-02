namespace Frank.Mapping;

public class MappingDefinitionNotFoundException : Exception
{
    public MappingDefinitionNotFoundException(Type from, Type to) : base($"No mapping definition found for {from.Name} to {to.Name}")
    {
    }
}