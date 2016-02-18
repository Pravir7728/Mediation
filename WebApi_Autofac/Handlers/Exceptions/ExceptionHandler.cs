using System;
using FluentValidation;
using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac.Handlers.Exceptions
{
    public class ExceptionHandler<TRequest, TResponse> : MediatR.IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILoggingHandler<TRequest, TResponse> _loggingHandler;
        private readonly MediatR.IRequestHandler<TRequest, TResponse> _requestHandler;

        public ExceptionHandler(ILoggingHandler<TRequest, TResponse> logginHandler,
            MediatR.IRequestHandler<TRequest, TResponse> requestHandler)
        {
            _loggingHandler = logginHandler;
            _requestHandler = requestHandler;
        }

        public TResponse Handle(TRequest request)
        {
            var response = default(TResponse);

            try
            {
                response = this._requestHandler.Handle(request);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this._loggingHandler.LogError(request, response, ex);
            }

            return response;
        }
    }

}