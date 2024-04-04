using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskProject.Service.UseCases.Products.Commands;
using TaskProject.Service.UseCases.Products.Queries;

namespace TaskProject.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateAsync([FromForm]CreateProductCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        public async ValueTask<IActionResult> DeleteAsync(int id)
        {
            var response = await _mediator.Send(new DeleteProductCommand() { Id = id });

            return Ok(response);
        }

        [HttpPut]
        public async ValueTask<IActionResult> UpdateAsync([FromForm]UpdateProductCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllAsync()
        {
            var response = await _mediator.Send(new GetAllProductQuery());

            return Ok(response);
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetProductByIdAsync(int Id)
        {
            var response = await _mediator.Send(new GetProductByIdQuery() { Id = Id });

            return Ok(response);
        }
    }
}