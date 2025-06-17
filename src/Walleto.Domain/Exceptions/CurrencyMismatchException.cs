using Walleto.Domain.Abstractions.Exceptions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Exceptions;

public class CurrencyMismatchException : DomainException
{
    public Currency ExpectedCurrency { get; }
    public Currency ActualCurrency { get; }

    public CurrencyMismatchException(Currency expectedCurrency, Currency actualCurrency)
        : base($"Currency mismatch. Expected {expectedCurrency.Code} but got {actualCurrency.Code}")
    {
        ExpectedCurrency = expectedCurrency;
        ActualCurrency = actualCurrency;
    }
}