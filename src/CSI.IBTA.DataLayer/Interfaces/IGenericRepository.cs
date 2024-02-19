using System.Linq.Expressions;

namespace CSI.IBTA.DataLayer.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> All();
        Task<T?> GetById(int id);
        Task<bool> Add(T entity);
        Task<bool> Delete(int id);
        bool Upsert(T entity);
        Task<IEnumerable<T>> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includes);
    }
}
