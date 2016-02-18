namespace WebApi_Autofac.Infrastructure.ProcessFlow
{
    public interface IPreRequestHandler<in TRequest>
    {
        void Handle(TRequest request);
    }
}