using LibrarySystem.Domain.Data;
using System.Linq.Expressions;

namespace LibrarySystem.Domain.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include);

        IQueryable<T> GetQueryable();
        LibraryDbContext Context { get; }

        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task SoftDeleteAsync(T entity);
        Task SaveAsync();
    }
}