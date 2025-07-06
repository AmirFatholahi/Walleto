using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class UserRegisteredEvent : DomainEvent
{
    public int UserID { get; }
    public string Email { get; }
    public string FullName { get; }

    public UserRegisteredEvent(int userID, string email, string fullName)
    {
        UserID = userID;
        Email = email;
        FullName = fullName;
    }
}

public class UserProfileUpdatedEvent : DomainEvent
{
    public int UserId { get; }
    public string NewFullName { get; }

    public UserProfileUpdatedEvent(int userID, string newFullName)
    {
        UserId = userID;
        NewFullName = newFullName;
    }
}

public class UserPasswordChangedEvent : DomainEvent
{
    public int UserID { get; }

    public UserPasswordChangedEvent(int userID)
    {
        UserID = userID;
    }
}

public class UserDeactivatedEvent : DomainEvent
{
    public int UserID { get; }

    public UserDeactivatedEvent(int userID)
    {
        UserID = userID;
    }
}

public class UserActivatedEvent : DomainEvent
{
    public int UserID { get; }

    public UserActivatedEvent(int userID)
    {
        UserID = userID;
    }
}