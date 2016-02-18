namespace WebApi_Autofac.Infrastructure.Processes
{
    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}