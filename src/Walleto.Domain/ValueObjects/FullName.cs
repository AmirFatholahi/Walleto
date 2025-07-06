using Walleto.Domain.Abstractions;

namespace Walleto.Domain.ValueObjects;

public class FullName : ValueObject
{
    public string fullName ;

    public FullName(FirstName firstName, LastName lastName)
    {

        fullName = $"{firstName.firstName} {lastName.lastName}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return fullName.ToLower();

    }
}