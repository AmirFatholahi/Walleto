using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class SubCategoryUpdatedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public Guid SubCategoryId { get; }
    public string NewName { get; }

    public SubCategoryUpdatedEvent(Guid categoryId, Guid subCategoryId, string newName)
    {
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        NewName = newName;
    }
}
