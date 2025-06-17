using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class SubCategoryAddedEvent : DomainEvent
{
    public Guid CategoryId { get; }
    public Guid SubCategoryId { get; }
    public string Name { get; }

    public SubCategoryAddedEvent(Guid categoryId, Guid subCategoryId, string name)
    {
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Name = name;
    }
}
