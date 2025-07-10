using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class FirstName : ValueObject
{
    public string Value { get; }
    public FirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        Value = firstName.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }

}

