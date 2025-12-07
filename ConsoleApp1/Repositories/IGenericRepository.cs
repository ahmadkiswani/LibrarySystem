using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LibrarySystem.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);

        Task AddAsync(T entity);
        void Remove(T entity);
        Task Update(T entity);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include);


        Task SaveAsync();
    }
}
