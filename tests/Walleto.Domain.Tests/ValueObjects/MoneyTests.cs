using FluentAssertions;
using Walleto.Domain.Entities;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;
using Walleto.Domain.Exceptions;
using Walleto.Domain.ValueObjects;
using Xunit;

namespace Walleto.Domain.Tests.ValueObjects;

public class MoneyTests
{
    [Fact]
    public void Constructor_WithNegativeAmount_ThrowsArgumentException()
    {
        // Arrange & Act
        var act = () => new Money(-100, Currency.IRR);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Amount cannot be negative*");
    }

    [Fact]
    public void Constructor_WithValidAmount_CreatesMoneyObject()
    {
        // Arrange & Act
        var money = new Money(1000.50m, Currency.IRR);

        // Assert
        money.Amount.Should().Be(1000.50m);
        money.Currency.Should().Be(Currency.IRR);
    }

    [Fact]
    public void Add_WithSameCurrency_ReturnsCorrectSum()
    {
        // Arrange
        var money1 = new Money(1000, Currency.IRR);
        var money2 = new Money(500, Currency.IRR);

        // Act
        var result = money1.Add(money2);

        // Assert
        result.Amount.Should().Be(1500);
        result.Currency.Should().Be(Currency.IRR);
    }

    [Fact]
    public void Add_WithDifferentCurrency_ThrowsInvalidOperationException()
    {
        // Arrange
        var money1 = new Money(1000, Currency.IRR);
        var money2 = new Money(100, Currency.USD);

        // Act
        var act = () => money1.Add(money2);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot add money in USD to money in IRR");
    }

    [Fact]
    public void Subtract_WithSufficientAmount_ReturnsCorrectDifference()
    {
        // Arrange
        var money1 = new Money(1000, Currency.IRR);
        var money2 = new Money(300, Currency.IRR);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.Amount.Should().Be(700);
        result.Currency.Should().Be(Currency.IRR);
    }

    [Fact]
    public void Subtract_ResultingInNegative_CreatesNegativeMoney()
    {
        // Arrange
        var money1 = new Money(300, Currency.IRR);
        var money2 = new Money(500, Currency.IRR);

        // Act
        var result = money1.Subtract(money2);

        // Assert
        result.Amount.Should().Be(-200);
    }

    [Theory]
    [InlineData(1000, 500, true)]
    [InlineData(500, 1000, false)]
    [InlineData(500, 500, false)]
    public void IsGreaterThan_ComparesCorrectly(decimal amount1, decimal amount2, bool expected)
    {
        // Arrange
        var money1 = new Money(amount1, Currency.IRR);
        var money2 = new Money(amount2, Currency.IRR);

        // Act
        var result = money1.IsGreaterThan(money2);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToString_ReturnsFormattedString()
    {
        // Arrange
        var money = new Money(1500000, Currency.IRR);

        // Act
        var result = money.ToString();

        // Assert
        result.Should().Be("1,500,000 IRR");
    }
}

