using System.Linq.Expressions;
using Inventory_Management_System.Data;
using Inventory_Management_System.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Inventory_Management_System.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly InventoryContext _InventoryContext;
        public GenericRepository(InventoryContext InventoryContext)
        {
            _InventoryContext = InventoryContext;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _InventoryContext.Set<TEntity>().AddAsync(entity);
        }
        public async Task<bool> Delete(Expression<Func<TEntity, bool>> Predicate)
        {
            TEntity? result = await _InventoryContext.Set<TEntity>().FirstOrDefaultAsync(Predicate);
            if (result is not null)
            {
                _InventoryContext.Set<TEntity>().Remove(result);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return a Tracked List OF Entity
        /// </summary>
        /// <param name="Selector">Name Of Navigation Property</param>
        /// <returns>Task<IEnumerable<TEntity>></returns>
        public async Task<IEnumerable<TEntity>> GetAllAsync(int page=1, int noOfItems = 10)
        {

            return await _InventoryContext.Set<TEntity>().Skip((page - 1)* noOfItems).Take(noOfItems).ToListAsync();
        }

        ///  <summary>
        ///  It's a Generic Method to Get All Items With Filter
        public IQueryable<TEntity> GetAllWithFilter(Expression<Func<TEntity, bool>> expression, int page = 1, int noOfItems = 10)
        {
            return _InventoryContext.Set<TEntity>().Where(expression).Skip((page - 1) * noOfItems).Take(noOfItems);
        }
        /// <summary>
        /// Return a Nullable Item Of Entity
        /// </summary>
        /// <param name="expression">Lambda Expression</param>
        /// <param name="Selector">Name Of Navigation Property</param>
        /// <returns>Task<TEntity?></returns>
        public async Task<TEntity?> GetItemAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _InventoryContext.Set<TEntity>().FirstOrDefaultAsync(expression);
        }

        public async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> Predicate, TEntity entity)
        {
            TEntity? existedItem = await _InventoryContext.Set<TEntity>().FirstOrDefaultAsync(Predicate);
            if (existedItem is not null)
            {
                var entry = _InventoryContext.Entry(existedItem);
                var newValues = _InventoryContext.Entry(entity);

                foreach (var property in newValues.Properties)
                {
                    // تخطي المفتاح الأساسي
                    if (property.Metadata.IsPrimaryKey()) continue;

                    entry.Property(property.Metadata.Name).CurrentValue = property.CurrentValue;
                }

                entry.State = EntityState.Modified;
                return true;
            }
            return false;
        }
    }
}

