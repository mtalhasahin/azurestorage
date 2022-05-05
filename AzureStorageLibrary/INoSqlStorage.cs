using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> GetAsync(string rowKey, string partitionKey);
        Task DeleteAsync(string rowKey, string partitionKey);
        Task<TEntity> UpdateAsync(TEntity entity);
        IQueryable<TEntity> AllAsync();
        IQueryable<TEntity> QueryAsync(Expression<Func<TEntity, bool>> query);
    }
}
