using MediatR;
using Sienna.Application.Builders.Email;
using Sienna.Application.Interfaces.Email;
using Sienna.Domain.Abstractions.Identity.Services;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Application.UseCases.Identity.ResetPassword
{
    public sealed class ResetPasswordHandler(
        IIdentityService service,
        IEmailQueue emailQueue) : IRequestHandler<ResetPasswordCommand, Result>
    {
        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await service.ChangePasswordAsync(request.Email, request.Token, request.NewPassword);

            if (result.IsSuccess)
            {
                var message = new MailMessageBuilder()
                    .AddRecipient(request.Email)
                    .AddSubject("Sua senha foi alterada.")
                    .AddPlainBody($"Sua senha foi alterada em {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                    .Build();

                await emailQueue.EnqueueAsync(message, cancellationToken);
            }

            return result;
        }
    }
}
