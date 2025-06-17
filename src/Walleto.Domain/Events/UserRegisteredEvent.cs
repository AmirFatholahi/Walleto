using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class UserRegisteredEvent : DomainEvent
{
    public Guid UserId { get; }
    public string Email { get; }
    public string FullName { get; }

    public UserRegisteredEvent(Guid userId, string email, string fullName)
    {
        UserId = userId;
        Email = email;
        FullName = fullName;
    }
}

public class UserProfileUpdatedEvent : DomainEvent
{
    public Guid UserId { get; }
    public string NewFullName { get; }

    public UserProfileUpdatedEvent(Guid userId, string newFullName)
    {
        UserId = userId;
        NewFullName = newFullName;
    }
}

public class UserPasswordChangedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserPasswordChangedEvent(Guid userId)
    {
        UserId = userId;
    }
}

public class UserDeactivatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserDeactivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}

public class UserActivatedEvent : DomainEvent
{
    public Guid UserId { get; }

    public UserActivatedEvent(Guid userId)
    {
        UserId = userId;
    }
}