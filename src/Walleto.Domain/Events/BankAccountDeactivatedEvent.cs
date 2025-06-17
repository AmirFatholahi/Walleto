using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class BankAccountDeactivatedEvent : DomainEvent
{
    public Guid AccountId { get; }

    public BankAccountDeactivatedEvent(Guid accountId)
    {
        AccountId = accountId;
    }
}
