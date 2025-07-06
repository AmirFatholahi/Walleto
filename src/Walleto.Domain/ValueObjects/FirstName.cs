using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class FirstName : ValueObject
{
    public string firstName { get; }
    public FirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        firstName = firstName.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return firstName.ToLower();
    }

}

