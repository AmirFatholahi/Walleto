using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class Currency : ValueObject
{
    public string Code { get; }
    public string Name { get; }
    public string Symbol { get; }

    private Currency(string code, string name, string symbol)
    {
        Code = code;
        Name = name;
        Symbol = symbol;
    }

    // Predefined currencies
    public static readonly Currency IRR = new("IRR", "Iranian Rial", "﷼");
    public static readonly Currency USD = new("USD", "US Dollar", "$");
    public static readonly Currency EUR = new("EUR", "Euro", "€");

    private static readonly Dictionary<string, Currency> Currencies = new()
    {
        { "IRR", IRR },
        { "USD", USD },
        { "EUR", EUR }
    };

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Currency code cannot be empty", nameof(code));

        if (!Currencies.TryGetValue(code.ToUpperInvariant(), out var currency))
            throw new ArgumentException($"Currency with code '{code}' is not supported", nameof(code));

        return currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}