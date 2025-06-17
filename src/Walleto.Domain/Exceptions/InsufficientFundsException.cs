using Walleto.Domain.Abstractions.Exceptions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Exceptions;

public class InsufficientFundsException : DomainException
{
    public Money CurrentBalance { get; }
    public Money RequestedAmount { get; }

    public InsufficientFundsException(Money currentBalance, Money requestedAmount)
        : base($"Insufficient funds. Current balance: {currentBalance}, Requested amount: {requestedAmount}")
    {
        CurrentBalance = currentBalance;
        RequestedAmount = requestedAmount;
    }
}