using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;

namespace WebApi.Handlers
{
    public class ValidatorHandler<TRequest, TResponse> : IAsyncRequestHandler<TRequest, TResponse>
        where TRequest : IAsyncRequest<TResponse>
    {
        private readonly IAsyncRequestHandler<TRequest, TResponse> _inner;
        private readonly IValidator<TRequest>[] _validators;

        public ValidatorHandler(
            IAsyncRequestHandler<TRequest, TResponse> inner,
            IValidator<TRequest>[] validators)
        {
            _inner = inner;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            var context = new ValidationContext(request);

            var failures = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);

            return await _inner.Handle(request);
        }
    }
}