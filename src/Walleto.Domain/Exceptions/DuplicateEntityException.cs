using Walleto.Domain.Abstractions.Exceptions;

namespace Walleto.Domain.Exceptions;

public class DuplicateEntityException : DomainException
{
    public string EntityName { get; }
    public string PropertyName { get; }
    public string PropertyValue { get; }

    public DuplicateEntityException(string entityName, string propertyName, string propertyValue)
        : base($"{entityName} with {propertyName} '{propertyValue}' already exists")
    {
        EntityName = entityName;
        PropertyName = propertyName;
        PropertyValue = propertyValue;
    }
}