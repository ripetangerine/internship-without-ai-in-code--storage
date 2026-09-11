using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using System.Diagnostics;

namespace Infra.Repositories
{
    public class ManagedIdentityStorageBlobRepository // TODO : 추후 인터페이스 구현(application
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ManagedIdentityStorageBlobRepository(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task UploadAsync(
            string containerName,
            string blobName,
            byte[] bytes,
            string? contentType = null
        )
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync(PublicAccessType.None);

            var blob = container.GetBlobClient(blobName);
            using var stream = new MemoryStream(bytes);

            var options = new BlobUploadOptions();
            if (!string.IsNullOrEmpty(contentType))
            {
                options.HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                };
            }
            await blob.UploadAsync(stream, options);
        }

        public async Task<string?> GetSasUrlAsync(string containerName, string blobName, int expiryHours = 1)
        {
            var blob = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobName);
            if (!await blob.ExistsAsync()) return null;

            var options = new BlobGetUserDelegationKeyOptions(
                DateTimeOffset.UtcNow.AddMinutes(-5)
            )
            {
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(expiryHours+1)
            };
            
            var delegationKey = await _blobServiceClient.GetUserDelegationKeyAsync(options);


            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = blobName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(expiryHours)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            var blobUriBuilder = new BlobUriBuilder(blob.Uri)
            {
                Sas = sasBuilder.ToSasQueryParameters(delegationKey, _blobServiceClient.AccountName)
            };

            return blobUriBuilder.ToUri().ToString();
        }

        public async Task<byte[]?> GetBytesAsync(string containerName, string blobName)
        {
            var blob = _blobServiceClient
                .GetBlobContainerClient(containerName).GetBlobClient(blobName);

            if (!await blob.ExistsAsync()) return null;

            using var ms = new MemoryStream();
            await blob.DownloadToAsync(ms);
            return ms.ToArray();
        }
    
        public async Task DeleteAsync(string containerName, string blobName)
        {
            var blob = _blobServiceClient.GetBlobContainerClient(containerName).GetBlobClient(blobName);
            await blob.DeleteIfExistsAsync();
        }
    }
}
