using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac.Handlers.Logging
{
    public class LoggingHandler<TRequest, TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILoggingHandler<TRequest, TResponse> _loggingHandler;
        private readonly MediatR.IRequestHandler<TRequest, TResponse> _requestHandler;

        public LoggingHandler(MediatR.IRequestHandler<TRequest, TResponse> requestHandler,
            ILoggingHandler<TRequest, TResponse> loggingHandler)
        {
            _requestHandler = requestHandler;
            _loggingHandler = loggingHandler;
        }

        public TResponse Handle(TRequest request)
        {
            _loggingHandler.LogRequestInfo<TRequest, TResponse>(request);

            var response = _requestHandler.Handle(request);

            _loggingHandler.LogResponseInfo(response);

            return response;
        }
    }
}