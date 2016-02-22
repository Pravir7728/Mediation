using System;
using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac.Handlers.Logging
{
    public class Logger<TRequest, TResponse> : ILoggingHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public void Handle(TRequest request, TResponse response)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(TRequest request, TResponse response)
        {
            throw new NotImplementedException();
        }

        public void LogError(TRequest request, TResponse response, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void LogRequestInfo<TRequest1, TResponse1>(TRequest1 request) where TRequest1 : IRequest<TResponse1>
        {
            throw new NotImplementedException();
        }

        public void LogResponseInfo<TResponse1>(TResponse1 response)
        {
            throw new NotImplementedException();
        }

        public void LogSql(string sql)
        {
            throw new NotImplementedException();
        }
    }
}