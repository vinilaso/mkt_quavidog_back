using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Identity.RegisterUser;

namespace Sienna.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterUserCommand request)
        {
            Guid createdUserId = await mediator.Send(request);
            return Created("create", createdUserId);
        }
    }
}
