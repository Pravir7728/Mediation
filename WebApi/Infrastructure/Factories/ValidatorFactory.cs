using System;
using Autofac;
using FluentValidation;

namespace WebApi.Infrastructure.Factories
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        private readonly IComponentContext _context;

        public ValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            object instance;
            if (_context.TryResolve(validatorType, out instance))
            {
                var validator = instance as IValidator;
                return validator;
            }

            return null;
        }
    }
}