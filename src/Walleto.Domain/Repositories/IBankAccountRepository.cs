using Walleto.Domain.Entities;

namespace Walleto.Domain.Repositories;

public interface IBankAccountRepository
{
    Task<BankAccount> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, string accountNumber, CancellationToken cancellationToken = default);
    Task AddAsync(BankAccount account, CancellationToken cancellationToken = default);
    Task UpdateAsync(BankAccount account, CancellationToken cancellationToken = default);
}