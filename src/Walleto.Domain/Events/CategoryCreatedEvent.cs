using Walleto.Domain.Abstractions;
using Walleto.Domain.Enums;

namespace Walleto.Domain.Events;

public class CategoryCreatedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public Guid UserId { get; }
    public string Name { get; }
    public CategoryType Type { get; }

    public CategoryCreatedEvent(Guid categoryId, Guid userId, string name, CategoryType type)
    {
        CategoryId = categoryId;
        UserId = userId;
        Name = name;
        Type = type;
    }
}
