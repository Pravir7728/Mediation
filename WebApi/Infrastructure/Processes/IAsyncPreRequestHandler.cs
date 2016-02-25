using System.Threading.Tasks;

namespace WebApi.Infrastructure.Processes
{
    public interface IAsyncPreRequestHandler<in TRequest>
    {
        Task Handle(TRequest request);
    }
}