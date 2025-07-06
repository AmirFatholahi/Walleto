using System.Transactions;
using Walleto.Domain.Abstractions;
using Walleto.Domain.Enums;
using Walleto.Domain.Events;
using Walleto.Domain.Exceptions;
using Walleto.Domain.ValueObjects;

namespace Walleto.Domain.Entities;

public class BankAccount : AggregateRoot
{
    private readonly List<Transaction> _transactions = new();

    public Guid UserId { get; private set; }
    public string AccountName { get; private set; } = null!;
    public string BankName { get; private set; } = null!;
    public BankAccountNumber AccountNumber { get; private set; } = null!;
    public Money Balance { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

    private BankAccount() { } // For EF Core

    public BankAccount(
        Guid userId,
        string accountName,
        string bankName,
        BankAccountNumber accountNumber,
        Currency currency)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("User ID cannot be empty", nameof(userId));

        if (string.IsNullOrWhiteSpace(accountName))
            throw new ArgumentException("Account name cannot be empty", nameof(accountName));

        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name cannot be empty", nameof(bankName));

        ID = Guid.NewGuid();
        UserId = userId;
        AccountName = accountName.Trim();
        BankName = bankName.Trim();
        AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
        Balance = Money.Zero(currency);
        IsActive = true;
        CreatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankAccountCreatedEvent(ID, userId, accountName, bankName));
    }

    public void RecordIncome(
        Money amount,
        Guid categoryId,
        Guid? subCategoryId,
        string description,
        DateTime? transactionDate = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot record transactions on inactive account");

        if (amount.Currency != Balance.Currency)
            throw new CurrencyMismatchException(Balance.Currency, amount.Currency);

        var transaction = new Transaction(
            TransactionType.Income,
            amount,
            categoryId,
            subCategoryId,
            description,
            transactionDate ?? DateTime.UtcNow);

        _transactions.Add(transaction);
        Balance = Balance.Add(amount);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new IncomeRecordedEvent(ID, transaction.ID, amount));
    }

    public void RecordExpense(
        Money amount,
        Guid categoryId,
        Guid? subCategoryId,
        string description,
        DateTime? transactionDate = null)
    {
        if (!IsActive)
            throw new InvalidOperationException("Cannot record transactions on inactive account");

        if (amount.Currency != Balance.Currency)
            throw new CurrencyMismatchException(Balance.Currency, amount.Currency);

        if (Balance.IsLessThan(amount))
            throw new InsufficientFundsException(Balance, amount);

        var transaction = new Transaction(
            TransactionType.Expense,
            amount,
            categoryId,
            subCategoryId,
            description,
            transactionDate ?? DateTime.UtcNow);

        _transactions.Add(transaction);
        Balance = Balance.Subtract(amount);
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new ExpenseRecordedEvent(ID, transaction.ID, amount));
    }

    public void UpdateAccountInfo(string accountName, string bankName)
    {
        if (string.IsNullOrWhiteSpace(accountName))
            throw new ArgumentException("Account name cannot be empty", nameof(accountName));

        if (string.IsNullOrWhiteSpace(bankName))
            throw new ArgumentException("Bank name cannot be empty", nameof(bankName));

        AccountName = accountName.Trim();
        BankName = bankName.Trim();
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankAccountUpdatedEvent(ID, accountName, bankName));
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Account is already deactivated");

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankAccountDeactivatedEvent(ID));
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Account is already active");

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankAccountActivatedEvent(ID));
    }

    // Method to set initial balance when importing from bank
    public void SetInitialBalance(Money balance)
    {
        if (_transactions.Any())
            throw new InvalidOperationException("Cannot set initial balance when transactions exist");

        if (balance.Currency != Balance.Currency)
            throw new CurrencyMismatchException(Balance.Currency, balance.Currency);

        Balance = balance;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new BankAccountBalanceSetEvent(ID, balance));
    }
}