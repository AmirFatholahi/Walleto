using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class CategoryDeactivatedEvent : DomainEvent
{
    public Guid CategoryId { get; }

    public CategoryDeactivatedEvent(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}
