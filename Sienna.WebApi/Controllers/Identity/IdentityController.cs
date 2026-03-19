using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Identity.Login;
using Sienna.Application.UseCases.Identity.RegisterUser;
using Sienna.Domain.Abstractions;
using Sienna.WebApi.Contracts.Identity;

namespace Sienna.WebApi.Controllers.Identity
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IMediator mediator) : SiennaController
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterUserCommand request)
        {
            Result<Guid> result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleFailure(result);

            return Created("create", result.Value);
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginCommand request)
        {
            var result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(new LoginResponse(result.Value));
        }
    }
}
