using FluentValidation.Results;

namespace WebApi_Autofac.Infrastructure.Processes
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T instance);
    }
}