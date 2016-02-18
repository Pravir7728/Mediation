﻿using System;
using MediatR;

namespace WebApi_Autofac.Handlers.Logging
{
    public class LogginHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public TResponse Handle(TRequest message)
        {
            throw new NotImplementedException();
        }
    }
}