using AquaCulture.Application.Interfaces.Repositories;
using AquaCulture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AquaCulture.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<Task> DeleteAsync(T entity)
        {
            var property = entity.GetType().GetProperty("IsDeleted");
            if (property != null)
            {
                property.SetValue(entity, true);
                _dbSet.Update(entity);
            }
            else
                _dbSet.Remove(entity); 

            return Task.CompletedTask;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
