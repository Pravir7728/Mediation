using MediatR;

namespace WebApi_Autofac.Infrastructure.Processes
{
    public interface IRequestHandler<in TRequest, out TResponse>
        where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest message);
    }
}