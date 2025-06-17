using Walleto.Domain.Abstractions;

namespace Walleto.Domain.Events;

public class BankAccountCreatedEvent : DomainEvent
{
    public Guid AccountId { get; }
    public Guid UserId { get; }
    public string AccountName { get; }
    public string BankName { get; }

    public BankAccountCreatedEvent(Guid accountId, Guid userId, string accountName, string bankName)
    {
        AccountId = accountId;
        UserId = userId;
        AccountName = accountName;
        BankName = bankName;
    }
}
