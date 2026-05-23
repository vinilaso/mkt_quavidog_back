using MediatR;

namespace Sienna.Application.Messaging
{
    public interface ICommand<out TResponse> : IRequest<TResponse>;

    public interface ICommand : IRequest;
}
