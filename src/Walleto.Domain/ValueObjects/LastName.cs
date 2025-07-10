using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class LastName : ValueObject
{
    public string Value { get;}

    public LastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        Value = lastName.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }

}
