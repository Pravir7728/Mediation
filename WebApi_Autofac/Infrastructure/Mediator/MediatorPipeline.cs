using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac.Infrastructure.Mediator
{
    public class MediatorPipeline<TRequest, TResponse>
        : MediatR.IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly MediatR.IRequestHandler<TRequest, TResponse> _inner;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly ILoggingHandler<TRequest, TResponse> _logger;

        public MediatorPipeline(MediatR.IRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            ILoggingHandler<TRequest, TResponse> logger
            )
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            _logger = logger;
        }

        public TResponse Handle(TRequest message)
        {
            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(message);
            }

            var result = _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(message, result);
            }

            return result;
        }
    }
}