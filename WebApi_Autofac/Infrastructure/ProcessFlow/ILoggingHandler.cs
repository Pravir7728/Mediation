namespace WebApi_Autofac.Handlers.Logging
{
    public interface ILoggingHandler<in TRequest, in TResponse>
    {
        void Handle(TRequest request, TResponse response);
    }
}