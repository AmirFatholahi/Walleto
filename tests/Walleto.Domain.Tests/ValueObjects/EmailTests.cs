using FluentAssertions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Tests.ValueObjects;
public class EmailTests
{
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithEmptyEmail_ThrowsArgumentException(string email)
    {
        // Arrange & Act
        var act = () => new Email(email);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email cannot be empty*");
    }

    [Theory]
    [InlineData("notanemail")]
    [InlineData("@example.com")]
    [InlineData("user@")]
    [InlineData("user@.com")]
    [InlineData("user.example.com")]
    public void Constructor_WithInvalidFormat_ThrowsArgumentException(string email)
    {
        // Arrange & Act
        var act = () => new Email(email);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid email format*");
    }

    [Theory]
    [InlineData("user@example.com", "user@example.com")]
    [InlineData("USER@EXAMPLE.COM", "user@example.com")]
    [InlineData("User@Example.Com", "user@example.com")]
    public void Constructor_WithValidEmail_CreatesEmailObjectWithLowerCase(string input, string expected)
    {
        // Arrange & Act
        var email = new Email(input);

        // Assert
        email.Value.Should().Be(expected);
    }

    [Fact]
    public void ImplicitOperator_ConvertsToString()
    {
        // Arrange
        var email = new Email("user@example.com");

        // Act
        string emailString = email;

        // Assert
        emailString.Should().Be("user@example.com");
    }
}
