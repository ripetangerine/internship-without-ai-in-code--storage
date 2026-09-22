using Azure.Data.Tables;
using Main.DTO.Upload;
using Main.Domain.Entities;
using Main.Domain.Interfaces;

namespace Main.Services
{
    public class TableStorageService
    {
        private readonly ITableRepository<BlobImageTableEntity> _tableBlobRepository;
        public TableStorageService(ITableRepository<BlobImageTableEntity> tableBlobRepository) {
            _tableBlobRepository = tableBlobRepository;
        }

        public async Task<BlobImageTableEntity?> GetByBlobName(string blobName)
        {
            return await _tableBlobRepository.GetEntityAsync(partitionKey: blobName, rowKey: "METADATA");
        }

        public async Task SaveBlobMetaAsync(string blobName, string fileUrl, string contentType, long fileSize)
        {
            var entity = new BlobImageTableEntity
            {
                PartitionKey = blobName,
                RowKey = "METADATA",
                FileUrl = fileUrl,
                ContentType = contentType,
                FileSizeByte = fileSize
            };

            await _tableBlobRepository.AddEntityAsync(entity);
        }
    }
}
