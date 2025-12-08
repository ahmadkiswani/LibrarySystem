using LibrarySystem.Domain.Data;
using LibrarySystem.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibrarySystem.Domain.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly LibraryDbContext _context;
        private readonly DbSet<T> _dbSet;

        public LibraryDbContext Context => _context;

        public GenericRepository(LibraryDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var entity = await _dbSet.AsNoTracking().FirstOrDefaultAsync(e =>
                EF.Property<int>(e, "Id") == id);

            if (entity is AuditLog audit && audit.IsDeleted)
                return null;

            return entity;
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (typeof(AuditLog).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e =>
                    !EF.Property<bool>(e, nameof(AuditLog.IsDeleted)));
            }

            return await query.ToListAsync();
        }


        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = _dbSet.AsNoTracking().Where(predicate);

            if (typeof(AuditLog).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e =>
                    !EF.Property<bool>(e, nameof(AuditLog.IsDeleted)));
            }

            return await query.ToListAsync();
        }


        public async Task<List<T>> FindAsync(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IQueryable<T>> include)
        {
            IQueryable<T> query = include(
                _dbSet.AsNoTracking().Where(predicate)
            );

            if (typeof(AuditLog).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e =>
                    !EF.Property<bool>(e, nameof(AuditLog.IsDeleted)));
            }

            return await query.ToListAsync();
        }


        public IQueryable<T> Query()
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (typeof(AuditLog).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e =>
                    !EF.Property<bool>(e, nameof(AuditLog.IsDeleted)));
            }

            return query;
        }


        public async Task AddAsync(T entity)
        {
            if (entity is AuditLog audit)
            {
                audit.CreatedBy = 1;
                audit.CreatedDate = DateTime.Now;
                audit.IsDeleted = false;
            }

            await _dbSet.AddAsync(entity);
        }


        public async Task UpdateAsync(T entity)
        {
            if (entity is AuditLog audit)
            {
                audit.LastModifiedBy = 1;
                audit.LastModifiedDate = DateTime.Now;
            }

            _dbSet.Update(entity);
        }


        public async Task SoftDeleteAsync(T entity)
        {
            if (entity is AuditLog audit)
            {
                audit.IsDeleted = true;
                audit.DeletedBy = 1;
                audit.DeletedDate = DateTime.Now;
            }

            _dbSet.Update(entity);
        }


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
