using Main.DTO.Upload;
using Main.Services;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers;

[Route("/api/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private FilesService _filesService;
    public FilesController(FilesService filesService)
    {
        _filesService = filesService;
    }

    [HttpPost("images")] 
    public async Task<IActionResult> UploadImageDirect([FromForm] IFormFile ProfileImage) // FromBody = 텍스트 전용
    {
        // 프론트에서 사진을 보내주면 blob 형식으로 백엔드에서 바로 업로드 하는거임.
        // TODO : dto, service logic
        // TODO : 비지니스 로직 옮기기
        //_blobRepo.UploadAsync()

        //using var reader = new StreamReader(req.ProfileImage);
        //var body = await reader.ReadToEndAsync();

        //Console.WriteLine(body);
        //return Ok();

        UploadImageResponse result = await _filesService.UploadImageFilesDirect(ProfileImage);
        return Ok(
            new
            {
                result.ImageUrl
            }
        );
    }


}

