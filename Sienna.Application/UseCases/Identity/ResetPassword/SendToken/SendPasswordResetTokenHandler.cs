using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Application.Interfaces.Email.Templates.UseCases.ResetPassword;
using Sienna.Domain.Abstractions.Identity.Services;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword.SendToken
{
    public sealed class SendPasswordResetTokenHandler(
        IIdentityService service,
        IEmailQueue queue) : IRequestHandler<SendPassowordResetTokenCommand, Result>
    {
        public async Task<Result> Handle(SendPassowordResetTokenCommand request, CancellationToken cancellationToken)
        {
            var token = await service.GeneratePasswordResetTokenAsync(request.Email);

            if (token.IsSuccess)
            {
                var message = new MailMessageBuilder()
                    .AddRecipient(request.Email)
                    .AddTemplate(new ResetPasswordEmailTemplate())
                    .WithVariables(new ResetPasswordVariables(token.Value))
                    .Build();

                await queue.EnqueueAsync(message, cancellationToken);
            }

            return token;
        }
    }
}
