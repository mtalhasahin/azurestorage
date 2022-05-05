using Microsoft.Azure.Cosmos.Table;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : TableEntity, new()
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly CloudTable _cloudTable;

        public TableStorage()
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(ConnectionString.AzureStorageConnectionString);

            _cloudTableClient = account.CreateCloudTableClient();
            _cloudTable = _cloudTableClient.GetTableReference(typeof(TEntity).Name);
            _cloudTable.CreateIfNotExistsAsync();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var operation = TableOperation.InsertOrMerge(entity);
            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> AllAsync()
        {
            return _cloudTable.CreateQuery<TEntity>().AsQueryable();
        }

        public async Task DeleteAsync(string rowKey, string partitionKey)
        {
            var entity = await GetAsync(rowKey, partitionKey);
            var operation =  TableOperation.Delete(entity);

            await _cloudTable.ExecuteAsync(operation);
        }

        public async Task<TEntity> GetAsync(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> QueryAsync(Expression<Func<TEntity, bool>> query)
        {
            return _cloudTable.CreateQuery<TEntity>().Where(query);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);
            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }
    }
}
