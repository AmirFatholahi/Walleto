using Walleto.Domain.Abstractions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Events;

public class BankAccountBalanceSetEvent : DomainEvent
{
    public Guid AccountId { get; }
    public Money Balance { get; }

    public BankAccountBalanceSetEvent(Guid accountId, Money balance)
    {
        AccountId = accountId;
        Balance = balance;
    }
}