using System.Web.WebPages;
using Azure.Storage.Blobs;
using Main.Infrastructure.Repositories;
using Main.DTO.Upload;
using Microsoft.Identity.Client;

namespace Main.Services
{
    public class FilesService
    {
        private readonly ManagedIdentityStorageBlobRepository _miStorageBlobRepository;
        private readonly ILogger<FilesService> _logger;
        private  readonly BlobContainerClient _containerClient;
        public FilesService(ManagedIdentityStorageBlobRepository miStorageBlobRepository, ILogger<FilesService> logger, BlobServiceClient blobServiceClient)
        {
            _miStorageBlobRepository = miStorageBlobRepository;
            _logger = logger;
            _containerClient = blobServiceClient.GetBlobContainerClient("test-container-blob");
        }

        public async Task<UploadImageResponse> UploadImageFilesDirect(IFormFile ProfileImage)
        {

            //using var reader = new StreamReader(req.ProfileImage);
            //var body = await reader.ReadToEndAsync();

            //Console.WriteLine(body);

            // 컨테이너이름, 블롭이름, 크기, 타입

            try
            {
                string CONTANINER_NAME = "test-container";
                string FILE_NAME = $"TEST_{DateTime.UtcNow:yyyyMMdd_HHmmss}";
                //if (dto.ProfileImage?.Length == 0 || dto.ProfileImage == null)
                //{
                //    throw new InvalidOperationException("파일을 넣어주세요"); // TODO : 더 정교한 코드로 바꾸자
                //}
                byte[] ImageByteSize;

                using (var memoryStream = new MemoryStream())
                {
                    await ProfileImage.CopyToAsync(memoryStream);
                    ImageByteSize = memoryStream.ToArray();
                }

                string ImagePath = await _miStorageBlobRepository.UploadAsync(CONTANINER_NAME, FILE_NAME, ImageByteSize);
                _logger.LogInformation("UploadImageFilesDirect:");

                return new UploadImageResponse(ImagePath);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e} << 스토리지 업로드 실패, FilesService:UploadImagesFilesDirect");
                throw;
            }
        }

        public async Task<GetUploadSasResponse> GenerateUploadSasUrlAsync(GetUploadSasRequest request)
        {

            string fileExtension = Path.GetExtension(request.FileName);
            string blobName = $"{Guid.NewGuid()}{fileExtension}"; //TODO : 이게 어떤식으로blob 들어가는지 보고 폴더 구조로
            string? resultSasUrl = await _miStorageBlobRepository.GetSasUrlAsync(_containerClient.Name, blobName);

            if (string.IsNullOrWhiteSpace(resultSasUrl))
                throw new Exception("result sas url is none");

            return new GetUploadSasResponse
            {
                SasUrl = resultSasUrl,
                BlobName = blobName
            };
        }

        public async Task<ConfirmUploadResponse> ConfirmUploadAsync(ConfirmUploadRequest request)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(request.BlobName);

            bool fileExist = await blobClient.ExistsAsync();
            if (!fileExist)
                throw new BadHttpRequestException("파일이 정상적으로 업로드 되지 않음");

            // TODO : DB 파일 메타 저장 > blobName, url, 유저 id

            return new ConfirmUploadResponse
            {
                FileName = blobClient.Uri.ToString()
            };

        }

    }
}
