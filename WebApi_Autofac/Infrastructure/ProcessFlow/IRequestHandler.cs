using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MediatR;

namespace WebApi_Autofac.Infrastructure.ProcessFlow
{
    public interface IRequestHandler<in TRequest, out TResponse>
      where TRequest : IRequest<TResponse>
    {
        TResponse Handle(TRequest message);
    }
}