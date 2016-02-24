using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using MediatR;
using WebApi_Autofac.Infrastructure.Processes;

namespace WebApi_Autofac.Infrastructure.Mediator
{
    public class MediatorPipeline<TRequest, TResponse>
        : Processes.IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Processes.IRequestHandler<TRequest, TResponse> _inner;
        private readonly ILoggingHandler<TRequest, TResponse> _logger;
        private readonly IPostRequestHandler<TRequest, TResponse>[] _postRequestHandlers;
        private readonly IPreRequestHandler<TRequest>[] _preRequestHandlers;
        private readonly FluentValidation.IValidator<TRequest>[] _validators;

        public MediatorPipeline(Processes.IRequestHandler<TRequest, TResponse> inner,
            IPreRequestHandler<TRequest>[] preRequestHandlers,
            IPostRequestHandler<TRequest, TResponse>[] postRequestHandlers,
            ILoggingHandler<TRequest, TResponse> logger, FluentValidation.IValidator<TRequest>[] validators
            )
        {
            _inner = inner;
            _preRequestHandlers = preRequestHandlers;
            _postRequestHandlers = postRequestHandlers;
            _logger = logger;
            _validators = validators;
        }

        public TResponse Handle(TRequest message)
        {
            var context = new ValidationContext(message);

            foreach (var preRequestHandler in _preRequestHandlers)
            {
                preRequestHandler.Handle(message);
            }

            var result = _inner.Handle(message);

            foreach (var postRequestHandler in _postRequestHandlers)
            {
                postRequestHandler.Handle(message, result);
            }

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(f => f != null)
                .ToList();


            if (failures.Any())
                throw new ValidationException(failures);

            return result;
        }
    }
}