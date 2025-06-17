using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class BankAccountUpdatedEvent : DomainEvent
{
    public Guid AccountId { get; }
    public string NewAccountName { get; }
    public string NewBankName { get; }

    public BankAccountUpdatedEvent(Guid accountId, string newAccountName, string newBankName)
    {
        AccountId = accountId;
        NewAccountName = newAccountName;
        NewBankName = newBankName;
    }
}
