using Walleto.Domain.Abstractions;
using Walleto.Domain.Events;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Entities;

public class User : AggregateRoot
{
    public Email Email { get; private set; } = null!;
    public Password Password { get; private set; } = null!;
    public PersonName Name { get; private set; } = null!;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public bool IsActive { get; private set; }

    private User() { } // For EF Core

    public User(Email email, Password password, PersonName name)
    {
        Id = Guid.NewGuid();
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        CreatedAt = DateTime.UtcNow;
        IsActive = true;

        AddDomainEvent(new UserRegisteredEvent(Id, email.Value, name.FullName));
    }

    public void UpdateProfile(PersonName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserProfileUpdatedEvent(Id, name.FullName));
    }

    public void ChangePassword(Password newPassword, string currentPassword)
    {
        if (!Password.Verify(currentPassword))
            throw new InvalidOperationException("Current password is incorrect");

        Password = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(Id));
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("User is already deactivated");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserDeactivatedEvent(Id));
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("User is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new UserActivatedEvent(Id));
    }

    public bool CanLogin(string password)
    {
        if (!IsActive)
            return false;

        return Password.Verify(password);
    }
}