using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class SubCategoryDeactivatedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public Guid SubCategoryId { get; }

    public SubCategoryDeactivatedEvent(Guid categoryId, Guid subCategoryId)
    {
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
    }
}
