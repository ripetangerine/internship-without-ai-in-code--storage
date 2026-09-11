using Infra.Domain.Entities;
using Infra.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Main.Controllers;


[Route("/api/files")]
[ApiController]
public class FilesController: ControllerBase
{
    //private readonly IMediator _mediator;
    //public FilesController(IMediator mediator) => _mediator = mediator;
    private ManagedIdentityStorageBlobRepository _blobRepo;
    public FilesController(ManagedIdentityStorageBlobRepository repo)
    {
        _blobRepo = repo;
    }

    [HttpPost("images")]
    public async Task<IActionResult> UploadImageFiles()
    {
        // 프론트에서 사진을 보내주면 blob 형식으로 백엔드에서 바로 업로드 하는거임.
        // TODO : dto, service logic
        // TODO : 비지니스 로직 옮기기
        //_blobRepo.UploadAsync()

        using var reader = new StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();

        Console.WriteLine(body);
        return Ok();
    }
    

}

