using System.Linq;
using FluentValidation;
using MediatR;

namespace WebApi_Autofac.Handlers.Validation
{
    public class ValidationHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IValidator<TRequest>[] _validators;

        public ValidationHandler(IRequestHandler<TRequest, TResponse> requestHandler, IValidator<TRequest>[] validators)
        {
            _requestHandler = requestHandler;
            _validators = validators;
        }

        public TResponse Handle(TRequest message)
        {
            var context = new ValidationContext(message);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return _requestHandler.Handle(message);
        }
    }
}