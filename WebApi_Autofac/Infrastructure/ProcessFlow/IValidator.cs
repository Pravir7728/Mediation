using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace WebApi_Autofac.Infrastructure.ProcessFlow
{
    public interface IValidator<in T>
    {
        ValidationResult Validate(T instance);
    }
}
