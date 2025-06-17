using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Entities;

public class SubCategory : Entity
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private SubCategory() { } // For EF Core

    internal SubCategory(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Subcategory name cannot be empty", nameof(name));

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    internal void Update(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Subcategory name cannot be empty", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    internal void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Subcategory is already deactivated");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    internal void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Subcategory is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}