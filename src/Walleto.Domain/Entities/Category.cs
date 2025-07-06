using Walleto.Domain.Abstractions;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;

namespace Walleto.Domain.Entities;

public class Category : AggregateRoot
{
    private readonly List<SubCategory> _subCategories = new();

    public Guid UserId { get; private set; }
    public string Name { get; private set; } = null!;
    public CategoryType Type { get; private set; }
    public string? Icon { get; private set; }
    public string? Color { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();

    private Category() { } // For EF Core

    public Category(
        Guid userId,
        string name,
        CategoryType type,
        string? icon = null,
        string? color = null)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));

        ID = Guid.NewGuid();
        UserId = userId;
        Name = name.Trim();
        Type = type;
        Icon = icon?.Trim();
        Color = color?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoryCreatedEvent(ID, userId, name, type));
    }

    public void UpdateInfo(string name, string? icon = null, string? color = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty", nameof(name));

        Name = name.Trim();
        Icon = icon?.Trim();
        Color = color?.Trim();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoryUpdatedEvent(ID, name));
    }

    public SubCategory AddSubCategory(string name, string? description = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot add subcategory to inactive category");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Subcategory name cannot be empty", nameof(name));

        if (_subCategories.Any(sc => sc.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Subcategory with name '{name}' already exists");

        var subCategory = new SubCategory(name, description);
        _subCategories.Add(subCategory);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new SubCategoryAddedEvent(ID, subCategory.ID, name));

        return subCategory;
    }

    public void UpdateSubCategory(Guid subCategoryId, string name, string? description = null)
    {
        var subCategory = _subCategories.FirstOrDefault(sc => sc.ID == subCategoryId);
        if (subCategory == null)
            throw new InvalidOperationException($"Subcategory with ID '{subCategoryId}' not found");

        if (_subCategories.Any(sc => sc.ID != subCategoryId && sc.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException($"Another subcategory with name '{name}' already exists");

        subCategory.Update(name, description);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new SubCategoryUpdatedEvent(ID, subCategoryId, name));
    }

    public void DeactivateSubCategory(Guid subCategoryId)
    {
        var subCategory = _subCategories.FirstOrDefault(sc => sc.ID == subCategoryId);
        if (subCategory == null)
            throw new InvalidOperationException($"Subcategory with ID '{subCategoryId}' not found");

        subCategory.Deactivate();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new SubCategoryDeactivatedEvent(ID, subCategoryId));
    }

    public void ActivateSubCategory(Guid subCategoryId)
    {
        var subCategory = _subCategories.FirstOrDefault(sc => sc.ID == subCategoryId);
        if (subCategory == null)
            throw new InvalidOperationException($"Subcategory with ID '{subCategoryId}' not found");

        subCategory.Activate();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new SubCategoryActivatedEvent(ID, subCategoryId));
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Category is already deactivated");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        // Deactivate all subcategories
        foreach (var subCategory in _subCategories.Where(sc => sc.IsActive))
        {
            subCategory.Deactivate();
        }

        AddDomainEvent(new CategoryDeactivatedEvent(ID));
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Category is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CategoryActivatedEvent(ID));
    }
}