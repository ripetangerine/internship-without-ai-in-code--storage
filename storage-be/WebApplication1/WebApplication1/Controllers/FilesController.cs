using Main.DTO.Upload;
using Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers;

[Route("/api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly FilesService _filesService;
    public FilesController(FilesService filesService)
    {
        _filesService = filesService;
    }

    [HttpPost("upload-direct")] 
    public async Task<IActionResult> UploadImageDirect(IFormFile file) // FromBody = 텍스트 전용
    {
        // 프론트에서 사진을 보내주면 blob 형식으로 백엔드에서 바로 업로드 하는거임.
        // TODO : dto, service logic
        // TODO : 비지니스 로직 옮기기
        // todo : 추가로 컨트롤러에서 챙겨야하는거 추가하기 (설계철학
        UploadImageResponse result = await _filesService.UploadImageFilesDirect(file);
        return Ok(
            new
            {
                result.ImageUrl
            }
        );
    }

    [HttpPost("sas-url")]
    public async Task<IActionResult> GetUploadSasUrl([FromBody] GetUploadSasRequest request)
    {
        GetUploadSasRequest result = await _filesService.GenerateUploadSasUrlAsync(request);
        return Ok(result);
    }

    [HttpPost("comfirm")]
    public async Task<IActionResult> ConfirmUpload([FromBody] ConfirmUploadRequest request)
    {
        ConfirmUploadResponse result = await _filesService.ConfirmUploadAsync(request);
        return Ok(request);
    }
}

