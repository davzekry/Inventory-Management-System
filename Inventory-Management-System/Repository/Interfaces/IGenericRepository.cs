using System.Linq.Expressions;

namespace Inventory_Management_System.Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);

        Task<bool> Delete(Expression<Func<TEntity, bool>> Predicate);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> Predicate, TEntity entity);
        Task<TEntity?> GetItemAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync(int page = 1, int amount = 10);
        IQueryable<TEntity> GetAllWithFilter(Expression<Func<TEntity, bool>> expression);

    }
}
