using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using Azure;
using Azure.Data.Tables;
using Main.Domain.Interfaces;


namespace Main.Infrastructure.Repositories;


public class TableStorageRepository<T> : ITableRepository<T> where T : class, ITableEntity, new()
{
    private readonly TableClient _tableClient;

    public TableStorageRepository(TableServiceClient tableServiceClient)
    {
        string tableName = GetTableName();
        _tableClient = tableServiceClient.GetTableClient(tableName);
        _tableClient.CreateIfNotExists(); // 인스턴스 1회만 생성
    }

    public async Task<Response> AddEntityAsync(T entity)
    {
        return await _tableClient.AddEntityAsync(entity);
    }

    public async Task<T?> GetEntityAsync(string partitionKey, string rowKey)
    {
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

    public async Task<IEnumerable<T>> GetEntitiesByPartitionKeyAsync(string partitionKey, int count)
    {
        var entities = new List<T>();
        string filter = $"PartitionKey eq '{partitionKey.Replace("'", "''")}'";

        await foreach (var page in _tableClient.QueryAsync<T>(filter).AsPages(pageSizeHint: count))
        {
            entities.AddRange(page.Values);
            if (entities.Count >= count)
                break;
        }

        return entities.Take(count).ToList();
    }

    public async Task<IEnumerable<T>> GetEntitiesByQueryAsync(Expression<Func<T, bool>> filter)
    {
        var entities = new List<T>();
        await foreach (var entity in _tableClient.QueryAsync(filter))
        {
            entities.Add(entity);
        }
        return entities;
    }

    public async Task<Response> UpdateEntityAsync(T entity, ETag ifMatch)
    {
        return await _tableClient.UpdateEntityAsync(entity, ifMatch, TableUpdateMode.Replace);
    }

    public async Task<Response> DeleteEntityAsync(string partitionKey, string rowKey, ETag ifMatch)
    {
        return await _tableClient.DeleteEntityAsync(partitionKey, rowKey, ifMatch);
    }

    private static string GetTableName()
    {
        var attribute = typeof(T).GetCustomAttribute<TableAttribute>();
        return attribute?.Name ?? typeof(T).Name;
    }
}

