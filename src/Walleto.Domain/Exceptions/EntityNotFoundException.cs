using Walleto.Domain.Abstractions.Exceptions;

namespace Walleto.Domain.Exceptions;

public class EntityNotFoundException : DomainException
{
    public string EntityName { get; }
    public Guid EntityId { get; }

    public EntityNotFoundException(string entityName, Guid entityId)
        : base($"{entityName} with ID '{entityId}' was not found")
    {
        EntityName = entityName;
        EntityId = entityId;
    }
}