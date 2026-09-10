using Microsoft.AspNetCore.Mvc; // TODO : 넴스 이거 맞는지 확인
using MediatR;

namespace Main.Controllers
{
    [Route("products")]
    [ApiController]
    public class ProductsController: ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;
    }
}
