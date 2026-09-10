using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Main.Controllers;


[Route("images")]
[ApiController]
public class FilesController: ControllerBase
{
    private readonly IMediator _mediator;
    public FilesController(IMediator mediator) => _mediator = mediator;


}

