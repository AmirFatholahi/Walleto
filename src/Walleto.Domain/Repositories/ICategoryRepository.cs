using Walleto.Domain.Entities;
using Walleto.Domain.Enums;

namespace Walleto.Domain.Repositories;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetByUserIdAndTypeAsync(Guid userId, CategoryType type, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid userId, string name, CategoryType type, CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task UpdateAsync(Category category, CancellationToken cancellationToken = default);
}