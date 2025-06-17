using FluentAssertions;
using Walleto.Domain.Entities;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;
using Xunit;

namespace Walleto.Domain.Tests.Entities;

public class CategoryTests
{
    private readonly Guid _userId = Guid.NewGuid();

    [Fact]
    public void Constructor_WithValidData_CreatesCategory()
    {
        // Arrange & Act
        var category = new Category(
            _userId,
            "Groceries",
            CategoryType.Expense,
            "🛒",
            "#FF5733");

        // Assert
        category.Id.Should().NotBeEmpty();
        category.UserId.Should().Be(_userId);
        category.Name.Should().Be("Groceries");
        category.Type.Should().Be(CategoryType.Expense);
        category.Icon.Should().Be("🛒");
        category.Color.Should().Be("#FF5733");
        category.IsActive.Should().BeTrue();
        category.SubCategories.Should().BeEmpty();
    }

    [Fact]
    public void AddSubCategory_WithValidName_AddsSubCategory()
    {
        // Arrange
        var category = new Category(_userId, "Food", CategoryType.Expense);
        category.ClearDomainEvents();

        // Act
        var subCategory = category.AddSubCategory("Restaurants", "Dining out expenses");

        // Assert
        category.SubCategories.Should().ContainSingle();
        subCategory.Name.Should().Be("Restaurants");
        subCategory.Description.Should().Be("Dining out expenses");
        subCategory.IsActive.Should().BeTrue();

        category.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<SubCategoryAddedEvent>();
    }

    [Fact]
    public void AddSubCategory_WithDuplicateName_ThrowsException()
    {
        // Arrange
        var category = new Category(_userId, "Food", CategoryType.Expense);
        category.AddSubCategory("Restaurants");

        // Act
        var act = () => category.AddSubCategory("Restaurants");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Subcategory with name 'Restaurants' already exists");
    }

    [Fact]
    public void AddSubCategory_ToInactiveCategory_ThrowsException()
    {
        // Arrange
        var category = new Category(_userId, "Food", CategoryType.Expense);
        category.Deactivate();

        // Act
        var act = () => category.AddSubCategory("Restaurants");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add subcategory to inactive category");
    }

    [Fact]
    public void UpdateSubCategory_WithValidData_UpdatesSubCategory()
    {
        // Arrange
        var category = new Category(_userId, "Food", CategoryType.Expense);
        var subCategory = category.AddSubCategory("Restaurant", "Dining");
        category.ClearDomainEvents();

        // Act
        category.UpdateSubCategory(subCategory.Id, "Restaurants", "Dining out");

        // Assert
        var updated = category.SubCategories.First();
        updated.Name.Should().Be("Restaurants");
        updated.Description.Should().Be("Dining out");

        category.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<SubCategoryUpdatedEvent>();
    }

    [Fact]
    public void Deactivate_ActiveCategory_DeactivatesCategoryAndSubCategories()
    {
        // Arrange
        var category = new Category(_userId, "Food", CategoryType.Expense);
        var sub1 = category.AddSubCategory("Restaurants");
        var sub2 = category.AddSubCategory("Groceries");
        category.ClearDomainEvents();

        // Act
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
        category.SubCategories.Should().AllSatisfy(sc => sc.IsActive.Should().BeFalse());

        category.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<CategoryDeactivatedEvent>();
    }

    [Theory]
    [InlineData("  Salary  ", "Salary")]
    [InlineData("BONUS", "BONUS")]
    [InlineData("  Travel  ", "Travel")]
    public void Constructor_TrimsWhitespace(string input, string expected)
    {
        // Arrange & Act
        var category = new Category(_userId, input, CategoryType.Income);

        // Assert
        category.Name.Should().Be(expected);
    }
}