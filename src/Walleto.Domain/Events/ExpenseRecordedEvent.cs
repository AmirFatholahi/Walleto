using Walleto.Domain.Abstractions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Events;

public class ExpenseRecordedEvent : DomainEvent
{
    public Guid AccountId { get; }
    public Guid TransactionId { get; }
    public Money Amount { get; }

    public ExpenseRecordedEvent(Guid accountId, Guid transactionId, Money amount)
    {
        AccountId = accountId;
        TransactionId = transactionId;
        Amount = amount;
    }
}
