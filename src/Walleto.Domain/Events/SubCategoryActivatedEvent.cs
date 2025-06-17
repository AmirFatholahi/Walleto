using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class SubCategoryActivatedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public Guid SubCategoryId { get; }

    public SubCategoryActivatedEvent(Guid categoryId, Guid subCategoryId)
    {
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
    }
}
