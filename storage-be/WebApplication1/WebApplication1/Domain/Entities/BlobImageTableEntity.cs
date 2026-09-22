using Azure;
using Azure.Data.Tables;

namespace Main.Domain.Entities
{
    public class BlobImageTableEntity: ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

        // 추가 데이터 속성
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
        public long FileSizeByte { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}
