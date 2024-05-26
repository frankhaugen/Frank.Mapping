using System.Linq.Expressions;
using System.Reflection;

namespace Frank.Mapping.Documents.Models;

public class PropertyMapping<T, TProp> : PropertyMapping
{
    public PropertyMapping(Expression<Func<T, TProp>> propertyExpression, ValuePath valuePath)
        : base(GetPropertyInfo(propertyExpression), valuePath, typeof(T))
    {
        ArgumentNullException.ThrowIfNull(propertyExpression, nameof(propertyExpression));
        ArgumentNullException.ThrowIfNull(valuePath, nameof(valuePath));
    }

    private static PropertyInfo GetPropertyInfo(Expression<Func<T, TProp>> propertyExpression)
    {
        ArgumentNullException.ThrowIfNull(propertyExpression, nameof(propertyExpression));
        var memberExpression = propertyExpression.Body as MemberExpression;
        if (memberExpression != null)
            return memberExpression?.Member as PropertyInfo
                   ?? throw new ArgumentException("Expression is not a valid property expression.");
        if (propertyExpression.Body is UnaryExpression { Operand: MemberExpression operand }) memberExpression = operand;

        return memberExpression?.Member as PropertyInfo
               ?? throw new ArgumentException("Expression is not a valid property expression.");
    }
}