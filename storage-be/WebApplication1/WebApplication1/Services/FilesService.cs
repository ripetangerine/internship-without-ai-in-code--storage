using Infra.Repositories;
using Main.DTO.Upload;

namespace Main.Services
{
    public class FilesService
    {
        private ManagedIdentityStorageBlobRepository _miStorageBlobRepository;
        private ILogger<FilesService> _logger;
        public FilesService(ManagedIdentityStorageBlobRepository miStorageBlobRepository, ILogger<FilesService> logger)
        {
            _miStorageBlobRepository = miStorageBlobRepository;
            _logger = logger;
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

    }
}
