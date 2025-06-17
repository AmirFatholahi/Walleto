using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class CategoryUpdatedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public string NewName { get; }

    public CategoryUpdatedEvent(Guid categoryId, string newName)
    {
        CategoryId = categoryId;
        NewName = newName;
    }
}
