using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class BankAccountNumber : ValueObject
{
    public string Value { get; }

    public BankAccountNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Bank account number cannot be empty", nameof(value));

        // Remove any spaces or dashes
        var cleaned = value.Replace(" ", "").Replace("-", "");

        if (cleaned.Length < 10 || cleaned.Length > 26)
            throw new ArgumentException("Bank account number must be between 10 and 26 digits", nameof(value));

        if (!cleaned.All(char.IsDigit))
            throw new ArgumentException("Bank account number must contain only digits", nameof(value));

        Value = cleaned;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}