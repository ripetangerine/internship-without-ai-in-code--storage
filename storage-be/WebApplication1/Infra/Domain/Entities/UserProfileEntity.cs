using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Infra.Domain.Entities
{
    [Table("UserProfiles")]
    public class UserProfileEntity: ITableEntity
    {
        public string PartitionKey { get; set; } = default!; // pk 2개 
        public string RowKey { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }


    public class ManagedIdentityStorageTableRepository<T> // TODO : 추후분리, 추상화
            where T : class, ITableEntity, new()
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly TableClient _tableClient;

        //public ManagedIdentityStorageTableRepository(TableServiceClient tableServiceClient)
        //{
        //    _tableServiceClient = tableServiceClient;
        //    _tableClient = _tableServiceClient.GetTableClient(GetTableName());
        //}

        public async Task<Response> AddEntityAsync(T entity)
        {
            await EnsureTableExistsAsync();
            return await _tableClient.AddEntityAsync(entity);
        }

        public async Task<T?> GetEntityAsync(string partitionKey, string rowKey)
        {
            await EnsureTableExistsAsync();

            try
            {
                var response = await _tableClient.GetEntityAsync<T>(partitionKey, rowKey);
                return response.Value;
            }
            catch (RequestFailedException ex) when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> GetEntitiesByQueryAsync(Expression<Func<T, bool>> filter)
        {
            await EnsureTableExistsAsync();

            var entities = new List<T>();
            await foreach (var entity in _tableClient.QueryAsync(filter))
            {
                entities.Add(entity);
            }

            return entities;
        }

        public async Task<IEnumerable<T>> GetEntitiesByPartitionKeyAsync(string partitionKey, int count)
        {
            await EnsureTableExistsAsync();

            var entities = new List<T>();
            var filter = $"PartitionKey eq '{partitionKey.Replace("'", "''")}'";

            await foreach (var page in _tableClient.QueryAsync<T>(filter).AsPages(pageSizeHint: count))
            {
                entities.AddRange(page.Values);
                if (entities.Count >= count)
                    break;
            }

            return entities.Take(count).ToList();
        }

        public async Task<Response> UpdateEntityAsync(T entity, ETag ifMatch)
        {
            await EnsureTableExistsAsync();
            return await _tableClient.UpdateEntityAsync(entity, ifMatch, TableUpdateMode.Replace);
        }

        public async Task<Response> DeleteEntityAsync(string partitionKey, string rowKey, ETag ifMatch)
        {
            await EnsureTableExistsAsync();
            return await _tableClient.DeleteEntityAsync(partitionKey, rowKey, ifMatch);
        }

        private async Task EnsureTableExistsAsync()
        {
            await _tableClient.CreateIfNotExistsAsync();
        }

        //private static string GetTableName()
        //{
        //    var attribute = typeof(T).GetCustomAttribute<TableAttribute>();
        //    return attribute?.Name ?? "EmptyTableName";
        //}
    }

}
