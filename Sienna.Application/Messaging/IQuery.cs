using MediatR;

namespace Sienna.Application.Messaging
{
    public interface IQuery<out TResponse> : IRequest<TResponse>;
}
