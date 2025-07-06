using Walleto.Domain.Abstractions;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Entities;

public class User : AggregateRoot
{
    private User() { } 

    public User( Email email, Password password,FirstName firstName,LastName lastName,int creatorId,BirthDate birthDate,Gender gender)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        BirthDate = birthDate ?? throw new ArgumentNullException(nameof(birthDate));


        Gender = gender;
        CreatorId = creatorId;
        CreationDateTime = DateTime.UtcNow;
        IsActive = true;
        IsDeleted = false;

        AddDomainEvent(new UserRegisteredEvent(ID, Email.Value, FullName.fullName));
    }


    public FirstName FirstName { get; private set; }
    
    public LastName LastName { get; private set; }
    
    public FullName FullName => new FullName(FirstName, LastName);

    public Email Email { get; private set; } = null!;
    
    public Password Password { get; private set; } = null!;

    public BirthDate BirthDate { get; private set; }

    public Gender Gender { get; private set; }





    public void UpdateProfile(FirstName firstName, LastName lastName, int modifierId)
    {
        FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
        LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
        ModifierId = modifierId;
        ModificationDateTime = DateTime.UtcNow;

        AddDomainEvent(new UserProfileUpdatedEvent(ID, FullName.fullName));
    }

    public void ChangePassword(Password newPassword, string currentPassword, int modifierId)
    {
        if (!Password.Verify(currentPassword))
            throw new InvalidOperationException("Current password is incorrect");

        Password = newPassword ?? throw new ArgumentNullException(nameof(newPassword));
        ModifierId = modifierId;
        ModificationDateTime = DateTime.UtcNow;

        AddDomainEvent(new UserPasswordChangedEvent(ID));
    }

    public void Deactivate(int modifierId)
    {
        if (!IsActive)
            throw new InvalidOperationException("User is already deactivated");

        IsActive = false;
        ModifierId = modifierId;
        ModificationDateTime = DateTime.UtcNow;

        AddDomainEvent(new UserDeactivatedEvent(ID));
    }

    public void Activate(int modifierId)
    {
        if (IsActive)
            throw new InvalidOperationException("User is already active");

        IsActive = true;
        ModifierId = modifierId;
        ModificationDateTime = DateTime.UtcNow;

        AddDomainEvent(new UserActivatedEvent(ID));
    }

    public bool CanLogin(string password)
    {
        if (!IsActive || IsDeleted)
            return false;

        return Password.Verify(password);
    }
}