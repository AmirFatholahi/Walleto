using FluentAssertions;
using Walleto.Domain.Entities;
using Walleto.Domain.Events;
using Walleto.Domain.ValueObjects;
using Xunit;

namespace Walleto.Domain.Tests.Entities;

public class UserTests
{
    private Email ValidEmail => new("test@example.com");
    private Password ValidPassword => Password.Create("Test123!");
    private PersonName ValidName => new("John", "Doe");

    [Fact]
    public void Constructor_WithValidData_CreatesUser()
    {
        // Arrange & Act
        var user = new User(ValidEmail, ValidPassword, ValidName);

        // Assert
        user.Id.Should().NotBeEmpty();
        user.Email.Should().Be(ValidEmail);
        user.Password.Should().Be(ValidPassword);
        user.Name.Should().Be(ValidName);
        user.IsActive.Should().BeTrue();
        user.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        user.UpdatedAt.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithValidData_RaisesUserRegisteredEvent()
    {
        // Arrange & Act
        var user = new User(ValidEmail, ValidPassword, ValidName);

        // Assert
        user.DomainEvents.Should().ContainSingle();
        var domainEvent = user.DomainEvents.First() as UserRegisteredEvent;
        domainEvent.Should().NotBeNull();
        domainEvent!.UserId.Should().Be(user.Id);
        domainEvent.Email.Should().Be(ValidEmail.Value);
        domainEvent.FullName.Should().Be(ValidName.FullName);
    }

    [Fact]
    public void ChangePassword_WithCorrectCurrentPassword_ChangesPassword()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);
        var newPassword = Password.Create("NewPassword123!");
        user.ClearDomainEvents();

        // Act
        user.ChangePassword(newPassword, "Test123!");

        // Assert
        user.Password.Should().Be(newPassword);
        user.UpdatedAt.Should().NotBeNull();
        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserPasswordChangedEvent>();
    }

    [Fact]
    public void ChangePassword_WithIncorrectCurrentPassword_ThrowsException()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);
        var newPassword = Password.Create("NewPassword123!");

        // Act
        var act = () => user.ChangePassword(newPassword, "WrongPassword");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Current password is incorrect");
    }

    [Fact]
    public void CanLogin_WithCorrectPasswordAndActiveUser_ReturnsTrue()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);

        // Act
        var result = user.CanLogin("Test123!");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void CanLogin_WithIncorrectPassword_ReturnsFalse()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);

        // Act
        var result = user.CanLogin("WrongPassword");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void CanLogin_WithDeactivatedUser_ReturnsFalse()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);
        user.Deactivate();

        // Act
        var result = user.CanLogin("Test123!");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Deactivate_ActiveUser_DeactivatesSuccessfully()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);
        user.ClearDomainEvents();

        // Act
        user.Deactivate();

        // Assert
        user.IsActive.Should().BeFalse();
        user.UpdatedAt.Should().NotBeNull();
        user.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<UserDeactivatedEvent>();
    }

    [Fact]
    public void Deactivate_AlreadyDeactivatedUser_ThrowsException()
    {
        // Arrange
        var user = new User(ValidEmail, ValidPassword, ValidName);
        user.Deactivate();

        // Act
        var act = () => user.Deactivate();

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User is already deactivated");
    }
}
