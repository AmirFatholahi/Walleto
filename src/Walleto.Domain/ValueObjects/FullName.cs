using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class FullName : ValueObject
{
    public string Value;

    public FullName(FirstName firstName, LastName lastName)
    {

        Value = $"{firstName.Value} {lastName.Value}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();

    }
}