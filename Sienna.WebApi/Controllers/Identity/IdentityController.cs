using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sienna.Application.UseCases.Identity.Login;
using Sienna.Application.UseCases.Identity.RegisterUser;
using Sienna.Domain.Abstractions;
using Sienna.WebApi.Contracts.Identity;

namespace Sienna.WebApi.Controllers.Identity
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController(IMediator mediator) : SiennaController
    {
        [HttpPost("create")]
        [ProducesResponseType<Guid>(StatusCodes.Status201Created, Description = "O usuário foi criado com sucesso no banco de dados.")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, Description = "Falha de validação nos parâmetros de entrada.")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict, Description = "Conflito de estado, como e-mail já registrado.")]
        public async Task<IActionResult> CreateUserAsync([FromBody] RegisterUserCommand request)
        {
            Result<Guid> result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleFailure(result);

            return Created(string.Empty, result.Value);
        }

        [HttpPost("login")]
        [ProducesResponseType<LoginResponse>(StatusCodes.Status200OK, Description = "O usuário com e-mail e senha informados foi encontrado e o token JWT foi gerado com sucesso.")]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized, Description = "Não foi encontrado usuário com e-mail e senha informados, ou o usuário está bloqueado.")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginCommand request)
        {
            var result = await mediator.Send(request);

            if (result.IsFailure)
                return HandleFailure(result);

            return Ok(new LoginResponse(result.Value));
        }
    }
}
