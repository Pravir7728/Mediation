using System.Threading.Tasks;

namespace WebApi.Infrastructure.Processes
{
    public interface IAsyncPostRequestHandler<in TRequest, in TResponse>
    {
        Task Handle(TRequest reqeust, TResponse response);
    }
}