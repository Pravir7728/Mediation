namespace WebApi_Autofac.Infrastructure.Processes
{
    public interface IPostRequestHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}