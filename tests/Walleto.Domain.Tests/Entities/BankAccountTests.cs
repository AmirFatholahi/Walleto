using FluentAssertions;
using Walleto.Domain.Entities;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;
using Walleto.Domain.Exceptions;
using Walleto.Domain.ValueObjects;
using Xunit;

namespace Walleto.Domain.Tests.Entities;

public class BankAccountTests
{
    private readonly Guid _userId = Guid.NewGuid();
    private readonly BankAccountNumber _accountNumber = new("1234567890");
    private readonly Currency _currency = Currency.IRR;

    [Fact]
    public void Constructor_WithValidData_CreatesBankAccount()
    {
        // Arrange & Act
        var account = new BankAccount(
            _userId,
            "My Savings",
            "Bank Melli",
            _accountNumber,
            _currency);

        // Assert
        account.Id.Should().NotBeEmpty();
        account.UserId.Should().Be(_userId);
        account.AccountName.Should().Be("My Savings");
        account.BankName.Should().Be("Bank Melli");
        account.AccountNumber.Should().Be(_accountNumber);
        account.Balance.Should().Be(Money.Zero(_currency));
        account.IsActive.Should().BeTrue();
        account.Transactions.Should().BeEmpty();
    }

    [Fact]
    public void RecordIncome_WithValidData_AddsTransactionAndUpdatesBalance()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        var amount = Money.FromRial(5_000_000);
        var categoryId = Guid.NewGuid();
        account.ClearDomainEvents();

        // Act
        account.RecordIncome(amount, categoryId, null, "Salary");

        // Assert
        account.Balance.Should().Be(amount);
        account.Transactions.Should().ContainSingle();

        var transaction = account.Transactions.First();
        transaction.Type.Should().Be(TransactionType.Income);
        transaction.Amount.Should().Be(amount);
        transaction.CategoryId.Should().Be(categoryId);
        transaction.Description.Should().Be("Salary");

        account.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<IncomeRecordedEvent>();
    }

    [Fact]
    public void RecordExpense_WithSufficientBalance_AddsTransactionAndUpdatesBalance()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        account.SetInitialBalance(Money.FromRial(5_000_000));
        var expenseAmount = Money.FromRial(1_000_000);
        var categoryId = Guid.NewGuid();
        account.ClearDomainEvents();

        // Act
        account.RecordExpense(expenseAmount, categoryId, null, "Groceries");

        // Assert
        account.Balance.Amount.Should().Be(4_000_000);
        account.Transactions.Should().ContainSingle();

        var transaction = account.Transactions.First();
        transaction.Type.Should().Be(TransactionType.Expense);
        transaction.Amount.Should().Be(expenseAmount);

        account.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<ExpenseRecordedEvent>();
    }

    [Fact]
    public void RecordExpense_WithInsufficientBalance_ThrowsException()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        account.SetInitialBalance(Money.FromRial(1_000_000));
        var expenseAmount = Money.FromRial(2_000_000);
        var categoryId = Guid.NewGuid();

        // Act
        var act = () => account.RecordExpense(expenseAmount, categoryId, null, "Large purchase");

        // Assert
        act.Should().Throw<InsufficientFundsException>()
            .Where(ex => ex.CurrentBalance.Amount == 1_000_000 && ex.RequestedAmount.Amount == 2_000_000);
    }

    [Fact]
    public void RecordTransaction_WithDifferentCurrency_ThrowsException()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, Currency.IRR);
        var usdAmount = new Money(100, Currency.USD);
        var categoryId = Guid.NewGuid();

        // Act
        var act = () => account.RecordIncome(usdAmount, categoryId, null, "Foreign income");

        // Assert
        act.Should().Throw<CurrencyMismatchException>()
            .Where(ex => ex.ExpectedCurrency == Currency.IRR && ex.ActualCurrency == Currency.USD);
    }

    [Fact]
    public void RecordTransaction_OnInactiveAccount_ThrowsException()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        account.Deactivate();
        var amount = Money.FromRial(1_000_000);
        var categoryId = Guid.NewGuid();

        // Act
        var act = () => account.RecordIncome(amount, categoryId, null, "Income");

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot record transactions on inactive account");
    }

    [Fact]
    public void SetInitialBalance_WithNoTransactions_SetsBalance()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        var initialBalance = Money.FromRial(10_000_000);
        account.ClearDomainEvents();

        // Act
        account.SetInitialBalance(initialBalance);

        // Assert
        account.Balance.Should().Be(initialBalance);
        account.DomainEvents.Should().ContainSingle()
            .Which.Should().BeOfType<BankAccountBalanceSetEvent>();
    }

    [Fact]
    public void SetInitialBalance_WithExistingTransactions_ThrowsException()
    {
        // Arrange
        var account = new BankAccount(_userId, "Savings", "Bank", _accountNumber, _currency);
        account.RecordIncome(Money.FromRial(1_000_000), Guid.NewGuid(), null, "Income");
        var initialBalance = Money.FromRial(10_000_000);

        // Act
        var act = () => account.SetInitialBalance(initialBalance);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Cannot set initial balance when transactions exist");
    }
}