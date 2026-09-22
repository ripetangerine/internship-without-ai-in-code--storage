using System.Linq.Expressions;
using Azure;
using Azure.Data.Tables;

namespace Main.Domain.Interfaces;

public interface ITableRepository<T> where T : class, ITableEntity, new()
{
    Task<Response> AddEntityAsync(T entity);
    Task<T?> GetEntityAsync(string partitionKey, string rowKey);
    Task<IEnumerable<T>> GetEntitiesByPartitionKeyAsync(string partitionKey, int count);
    Task<IEnumerable<T>> GetEntitiesByQueryAsync(Expression<Func<T, bool>> filter);
    Task<Response> UpdateEntityAsync(T entity, ETag ifMatch);
    Task<Response> DeleteEntityAsync(string partitionKey, string rowKey, ETag ifMatch);
}