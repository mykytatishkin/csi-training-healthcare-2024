using CSI.IBTA.DataLayer.Models;
using CSI.IBTA.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace CSI.IBTA.DataLayer.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected CsiHealthcare2024Context context;
        internal DbSet<T> dbSet;
        public readonly ILogger _logger;

        public GenericRepository(
            CsiHealthcare2024Context context,
            ILogger logger)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
            _logger = logger;
        }

        public virtual async Task<T?> GetById(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual async Task<bool> Add(T entity)
        {
            await dbSet.AddAsync(entity);
            return true;
        }

        public virtual async Task<bool> Delete(int id)
        {
            var entity = await dbSet.FindAsync(id);

            if (entity == null)
            {
                return false;
            }

            dbSet.Remove(entity);
            return true;
        }

        public virtual async Task<IEnumerable<T>> All()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await dbSet.Where(predicate).ToListAsync();
        }

        public abstract Task<bool> Upsert(T entity);
    }
}
