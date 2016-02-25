using System.Web.Http;
using Autofac;
using FluentValidation;
using FluentValidation.WebApi;
using WebApi.Features;
using WebApi.Infrastructure;

namespace WebApi.App_Start
{
    public class FluentValidationConfig
    {
        public static void RegisterValidation(HttpConfiguration config)
        {
            FluentValidationModelValidatorProvider.Configure(config, provider => provider.ValidatorFactory = new ModelValidatorFactory());
        }
    }
}