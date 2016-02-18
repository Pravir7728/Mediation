using System;
using MediatR;

namespace WebApi_Autofac.Infrastructure.Processes
{
    public interface ILoggingHandler<in TRequest, in TResponse> where TRequest : IRequest<TResponse>
    {
        void Handle(TRequest request, TResponse response);
        void LogInfo(TRequest request, TResponse response);
        void LogError(TRequest request, TResponse response, Exception ex);
        void LogRequestInfo<TRequest, TResponse>(TRequest request) where TRequest : IRequest<TResponse>;
        void LogResponseInfo<TResponse>(TResponse response);
        void LogSql(string sql);
    }
}