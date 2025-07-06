using Walleto.Domain.Abstractions;
using Walleto.Domain.Enums;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Entities;

public class Transaction : Entity
{
    public TransactionType Type { get; private set; }
    public Money Amount { get; private set; } = null!;
    public Guid CategoryId { get; private set; }
    public Guid? SubCategoryId { get; private set; }
    public string Description { get; private set; } = null!;
    public DateTime TransactionDate { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Transaction() { } // For EF Core

    internal Transaction(
        TransactionType type,
        Money amount,
        Guid categoryId,
        Guid? subCategoryId,
        string description,
        DateTime transactionDate)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Category ID cannot be empty", nameof(categoryId));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));

        ID = Guid.NewGuid();
        Type = type;
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Description = description.Trim();
        TransactionDate = transactionDate;
        CreatedAt = DateTime.UtcNow;
    }
}