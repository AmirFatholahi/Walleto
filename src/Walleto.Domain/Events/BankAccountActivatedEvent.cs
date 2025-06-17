using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class BankAccountActivatedEvent : DomainEvent
{
    public Guid AccountId { get; }

    public BankAccountActivatedEvent(Guid accountId)
    {
        AccountId = accountId;
    }
}
