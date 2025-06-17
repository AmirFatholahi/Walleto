using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class CategoryActivatedEvent : DomainEvent
{
    public Guid CategoryId { get; }

    public CategoryActivatedEvent(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}