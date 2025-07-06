using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class LastName : ValueObject
{
    public string lastName { get;}

    public LastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        lastName = lastName.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return lastName.ToLower();
    }

}
