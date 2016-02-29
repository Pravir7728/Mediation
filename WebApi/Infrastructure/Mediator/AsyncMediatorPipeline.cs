using System.Threading.Tasks;
using log4net;
using log4net.Core;
using log4net.Repository.Hierarchy;
using MediatR;
using WebApi.Infrastructure.Processes;

namespace WebApi.Infrastructure.Mediator
{
    public class AsyncMediatorPipeline<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> inner;
        private readonly IAsyncPostRequestHandler<TRequest, TResponse>[] postRequestHandlers;
        private readonly IAsyncPreRequestHandler<TRequest>[] preRequestHandlers;

        public AsyncMediatorPipeline(IAsyncRequestHandler<TRequest, TResponse> inner,
            IAsyncPreRequestHandler<TRequest>[] preRequestHandlers,
            IAsyncPostRequestHandler<TRequest, TResponse>[] postRequestHandlers)
        {
            this.inner = inner;
            this.preRequestHandlers = preRequestHandlers;
            this.postRequestHandlers = postRequestHandlers;
        }

        public ILog Logger { get; set; }

        public async Task<TResponse> Handle(TRequest message)
        {
            Logger.Debug("Mediator Debug");

            foreach (var preRequestHandler in preRequestHandlers)
            {
                await preRequestHandler.Handle(message);
            }

            var result = await inner.Handle(message);

            foreach (var postRequestHandler in postRequestHandlers)
            {
                await postRequestHandler.Handle(message, result);
            }
            return result;
        }
    }
}